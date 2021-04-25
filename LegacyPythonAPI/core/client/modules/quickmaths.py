from __future__ import annotations
import re
import random
import time
import threading

levels:list[dict] = [
  {
    "symbols": ["+", "-"],
    "range1": 1,
    "range2": 10,
  },
  {
    "range1": 10,
    "range2": 20,
    "symbols": ["+", "-"]
  },
  {
    "range1": 10,
    "range2": 20,
    "symbols": ["+", "-", "*"]
  },
  {
    "range1": 30,
    "range2": 50,
    "symbols": ["+", "-", "*"]
  },
  {
    "range1": 50,
    "range2": 100,
    "symbols": ["+", "-", "*"]
  }
]

def level(range1:int, range2:int, symbols:list[str]=["+", "-"]) -> tuple[int, int, str]:
  num1 = random.randint(range1, range2)
  num2 = random.randint(range1, range2)
  cal_symbol = random.choice(symbols)
  return num1, num2, cal_symbol

def calculation(num1:int, num2:int, cal_symbol:str) -> int:
  return eval(f"{num1} {cal_symbol} {num2}")

def handle(text, Mic, Agent):
  endgame = False
  rounds = 0
  start_time = time.time()
  total_time = 120 
  Mic.say("Welcome to QuickMaths, say start if you want to start level_details new game")
  
  user_input = Mic.active_listen()

  while True:
    user_input = Mic.active_listen()
    if user_input != "start":
      continue
    elif user_input == "start":
      break

  while endgame == False:
    rounds += 1
    level_details:tuple[int, int, str]
    if rounds < 6:
      level_details = level(**levels[0])
    elif rounds < 11:
      level_details = level(**levels[1])
    elif rounds < 20:
      level_details = level(**levels[2])
    elif rounds < 40:
      level_details = level(**levels[3])
    else:
      level_details = level(**levels[4])

    num1, num2, cal_symbol = level_details # type: ignore
    ans = calculation(num1, num2, cal_symbol)

    time_past = time.time() - start_time
    time_left = total_time - time_past

    Agent._print(f"answer for {num1} {cal_symbol} {num2}? \t{round(time_left)}\n")
    while True:
      if time_left > 0:
        while True:
          user_response = Mic.active_listen()

          if user_response.isdigit():
            user_ans = int(user_response)
            Agent._print(user_response)

            if user_ans == ans:
              total_time += 5
              Agent._print("good")
              rounds += 1
            else:
              total_time -= 5
              Agent._print(f"bad {ans}")

            break

          else:
            user_ans = user_response
            total_time -= 5

      else:
        Agent._print(f"Game Over Your Score is {rounds}")
        endgame = True
        break

def isValid(text):
  # check if text is valid
  return bool(re.search(r'\bquick | maths\b', text, re.IGNORECASE))



