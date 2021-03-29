import re
import random
import time
import threading


def level1():
  num1 = random.randint(1, 10)
  num2 = random.randint(1, 10)
  symbols = ["+", "-"]
  cal_symbol = random.choice(symbols)
  return num1, num2, cal_symbol

def level2():
  num1 = random.randint(10, 20)
  num2 = random.randint(10, 20)
  symbols = ["+", "-"]
  cal_symbol = random.choice(symbols)
  return num1, num2, cal_symbol

def level3():
  num1 = random.randint(10, 20)
  num2 = random.randint(10, 20)
  symbols = ["+", "-", "*"]
  cal_symbol = random.choice(symbols)
  return num1, num2, cal_symbol

def level4():
  num1= random.randint(30, 50)
  num2 = random.randint(30, 50)
  symbols = ["+", "-", "*"]
  cal_symbol = random.choice(symbols)
  return num1, num2, cal_symbol

def level5():
  num1 = random.randint(50, 100)
  num2 = random.randint(50, 100)
  symbols = ["+", "-", "*"]
  cal_symbol = random.choice(symbols)
  return num1, num2, cal_symbol

def calculation(num1, num2, cal_symbol):
  if cal_symbol == "+":
    ans = num1 + num2
  elif cal_symbol == "-":
    ans = num1 - num2
  elif cal_symbol == "*":
    ans = num1 * num2
  elif cal_symbol == "/":
    ans = num1 / num2
  return ans

def handle(text, Mic, Agent):
  endgame = False
  rounds = 0
  start_time = time.time()
  total_time = 120 
  Mic.say("Welcome to QuickMaths, say start if you want to start a new game")
  
  user_input = Mic.active_listen()

  while True:
    user_input = Mic.active_listen()
    if user_input != "start":
      continue
    elif user_input == "start":
      break

  while endgame == False:
    rounds += 1
    if rounds < 6:
      a = level1()
    elif rounds < 11:
      a = level2()
    elif rounds < 20:
      a = level3()
    elif rounds < 40:
      a = level4()
    elif rounds < 50:
      a = level5()

    num1, num2, cal_symbol = a
    ans = calculation(num1, num2, cal_symbol)

    time_past = time.time() - start_time
    time_left = total_time - time_past

    print(f"answer for {num1} {cal_symbol} {num2}? \t{round(time_left)}\n")
    while True:
      if time_left > 0:
        while True:
          userresponse = Mic.active_listen()
          if userresponse == int:
            userans = userresponse
            print (userresponse)
            break
          elif userresponse != int:
            try:
              userans = int(userresponse)
              print (userresponse)
              break
            except:
              print (userresponse)
              continue
        if userans == ans:
          total_time += 5
          print("good")
          rounds += 1
          break
        else:
          total_time -= 5
          print(f"bad {ans}")
          break
      else:
        print(f"Game Over Your Score is {rounds}")
        endgame = True
        break

def isValid(text):
  # check if text is valid
  return (bool(re.search(r'\bquick | maths\b', text, re.IGNORECASE)))


# if user_input == "start"



