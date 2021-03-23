# This program is free software; you can redistribute it and/or
# modify it under the terms of the GNU General Public License
# as published by the Free Software Foundation; either version 2
# of the License, or (at your option) any later version.

# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.

# You should have received a copy of the GNU General Public License
# along with this program; if not, write to the Free Software Foundation,
# Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.

# The Original Code is Copyright (C) 2020 Voxell Technologies.
# All rights reserved.

import re
import bs4
import requests
import string

PUNCS = list(string.punctuation)
PUNCS.append("’")
text = 1
ALPHABETS = list(string.ascii_letters)
result_title = []
def separete_chap_num(text):
  split_symbol = " - "
  if ":" in text:
    split_symbol = ": "
  elif "-" in text:
    split_symbol = " - "
  else:
    return text, ""

  chap = text.split(split_symbol)[0]
  title = text.split(split_symbol)[1].lower()

  chap = re.search("\s+[0-9]+\s+", chap)

  return chap, title

def clean_text(text):
  for p in PUNCS:
    text = text.replace(p, "")
  text = text.strip()
  return text.replace(" ", "-")

def search(text, result_title = []):
  user_input = 'hidden marriage'
  search_input = user_input.replace(" ", "+")
  search = requests.get(f"https://novelfull.com/search?keyword={search_input}")

  soup_search = bs4.BeautifulSoup(search.text, "html.parser")
  find_results = soup_search.find_all("img",alt=True)

  for i in find_results:
    a = i.get("alt")
    b = clean_text(a)
    c = b.lower()
    result_title.append(c)

  return result_title

def content(text, result_title, dictionary):
  search(text, result_title)
  user_chap = '1'
  user_title = dictionary[user_chap]

  chap_link = requests.get(f"https://novelfull.com/{result_title[0]}/chapter-{user_chap}-{user_title}.html")
  soup_chap = bs4.BeautifulSoup(chap_link.text, "html.parser")

  find_content = soup_chap.find_all("p")


  for i in find_content:
    story = i.getText()
  return story

title_list = []
# text, Mic, Agent
def handle():
  novel_links = "https://novelfull.com/"

  res_novel = requests.get("https://novelfull.com/")
  soup_novel = bs4.BeautifulSoup(res_novel.text, "html.parser")

  find_latest = soup_novel.find_all("div", class_="col-xs-9 col-sm-6 col-md-5 col-title")

  links_bef = []
  links_aft = []
  title_list = []
  latest_releases = []

  for i in find_latest:
    # print(i.getText())
    links_bef.append(i.a["href"])
    latest_releases.append(i.getText())

  # print(latest_releases)

  for i in links_bef:
    i = i.replace(".html", "")
    links_aft.append(i)

  book_link = requests.get(f"https://novelfull.com{links_bef[0]}")
  soup_book = bs4.BeautifulSoup(book_link.text, "html.parser")
  find_lastpage = soup_book.find("li", class_="last")
  lastpage_no = find_lastpage.a["href"].split("?page=")
  maxpage = int(lastpage_no[1].split("&per-page=50")[0])

  chap_num_list = []
  title_info_list = []
  # result_title = []

  print(latest_releases, "If you wish to search for any book, say search, else" )
  user_voice = input("Say search:")
  user_voice = user_voice.lower()
  
  latest_release_index = 0
  latest_release_dictionary = {}

  for i in latest_releases:
    latest_release_index += 1
    latest_release_dictionary[latest_release_index] = i
  
  # print("test")
  # print(latest_release_dictionary[1])

  if user_voice == "search":
    user_answer = input("Say book name:")
    user_input = user_answer.lower()
    search(user_input, result_title)
    print(result_title, "please state the number of the book you wish to read, for example 1 for the first book")
    title_no = int(input())
    book_title = result_title[title_no]
      
  for page in range(maxpage):
    # print(page)
    book_link = requests.get(f"https://novelfull.com/index.php/{book_title}.html?page={page+1}&per-page=50")
    soup_book = bs4.BeautifulSoup(book_link.text, "html.parser")
    find_title = soup_book.find_all("a",title=True)
    
    
    # title_list = []
    for i in find_title:
      title_list.append(i["title"])

    for i in title_list:
      if i.startswith("zzz"):
        continue
      elif i.startswith("Chapter"):
        i = i.replace("Chapter ", "")
        chap, title = separete_chap_num(i)
        title = clean_text(title)
        chap = clean_text(chap)

        chap_num_list.append(chap)
        title_info_list.append(title)
    
        # print(f'chap: {chap} title: {title}')
        
      else:
        continue

    for i in chap_num_list:
      print(int(i))

    
    dictionary = dict(zip(chap_num_list, title_info_list))
  # print(chap_num_list)
  


  

def isValid(text):
  # check if text is valid
  return (bool(re.search(r'\bnovel\b', text, re.IGNORECASE)))


# if __name__ == '__main__':
#   print(isValid("novel"))
# search(text, result_title = [])
# print(result_title)
handle()