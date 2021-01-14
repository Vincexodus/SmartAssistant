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

import sqlite3
from pylogix import PLC

class LogixController(PLC):
  '''
  A controller that is able to control devices through PLC.
  It also contains a database that stores all the tags that has been defined by the user.
  '''

  def __init__(self, Agent):
    super().__init__()
    self.Agent = Agent
    self.IPAddress = self.Agent.controller_ip
    self.GetTagList() # populate self.TagList
    self.TagNames = [t.TagName for t in self.TagList]
    self.DataTypes = [t.DataType for t in self.TagList]

    self.database_filename = f"{self.Agent.TEMP_PATH}\\devices.db"

    self.connection = sqlite3.connect(self.database_filename)
    self.cursor = self.connection.cursor()
    self.create_table()
    self.Agent._print2(f"Connected to '{self.database_filename}'!")

  def __str__(self):
    sep = "="*len("Controller")
    return f"Controller{sep}\nIP Address: {self.IPAddress}\nDatabase: {self.database_filename}\nCategories: [Name, TagName, DataType, Group, Category, Description]"

  def create_table(self):
    '''
    Creates a table containing 6 collumns:
      - Name
      - TagName
      - DataType
      - Group
      - Category
      - Description
    '''
    try:
      self.cursor.execute("""CREATE TABLE TagList(Name        TEXT NOT NULL,
                                                  TagName     TEXT NOT NULL,
                                                  DataType    TEXT NOT NULL,
                                                  Group       TEXT NOT NULL,
                                                  Category    TEXT NOT NULL,
                                                  Description TEXT NOT NULL)""")
    except Exception as e:
      # in case if the table has been created before
      self.Agent._print2(e)

  def insert_row(self, Name, TagName, DataType, Group, Category, Description):
    # Insert a row into the table
    if Description is None:
      Description = "-"

    if TagName in self.TagNames:
      self.cursor.execute("INSERT INTO TagList(Name, TagName, DataType, Group, Category, Description) VALUES (?, ?, ?, ?, ?, ?)",
                          (Name, TagName, DataType, Group, Category, Description, ))
      self.connection.commit()
    else:
      raise ValueError("`TagName` not found in `self.TagNames` which contains all available tag names")

  def insert_multi_rows(self, Name, TagName, DataType, Group, Category, Description):
    # Insert a row into the table
    # used to update a large amount of rows
    for i, d in enumerate(Description):
      if d is None:
        Description[i] = "-"

    for N, TN, DT, GRP, CAT, DES in zip(Name, TagName, DataType, Group, Category, Description):
      if TN in self.TagNames:
        self.cursor.execute("INSERT INTO TagList(Name, TagName, DataType, Group, Category, Description) VALUES (?, ?, ?, ?, ?, ?)",
                            (N, TN, DT, GRP, CAT, DES, ))
      else:
        raise ValueError("`TagName` not found in `self.TagNames` which contains all available tag names")
    self.connection.commit()

  def retrieve_row_by_name(self, Name):
    # retrieve row content based on the `Name` given
    self.cursor.execute("SELECT TagName, DataType, Group, Category, Description FROM TagList WHERE Name = ?", (Name, ))
    TagName, DataType, Group, Category, Description = self.cursor.fetchone()[0]

    return Name, TagName, DataType, Group, Category, Description

  def retrieve_row_by_tagname(self, TagName):
    # retrieve row content based on the `TagName` given
    self.cursor.execute("SELECT Name, DataType, Group, Category, Description FROM TagList WHERE TagName = ?", (TagName, ))
    Name, DataType, Group, Category, Description = self.cursor.fetchone()[0]

    return Name, TagName, DataType, Group, Category, Description

  def retrieve_rows_by_group(self, Group):
    # retrieve all row content based on the `Group` given
    self.cursor.execute("SELECT Name, TagName, DataType, Category, Description FROM TagList WHERE Group = ?", (Group, ))
    fetches = self.cursor.fetchall()
    fetches = [list(f) for f in fetches]

    return [(f[:3] + [Group] + f[3:]) for f in fetches]

  def retrieve_rows_by_category(self, Category):
    # retrieve all row content based on the `Category` given
    self.cursor.execute("SELECT Name, TagName, DataType, Group, Description FROM TagList WHERE Category = ?", (Category, ))
    fetches = self.cursor.fetchall()
    fetches = [list(f) for f in fetches]

    return [(f[:4] + [Group] + f[4:]) for f in fetches]

  def retrieve_all(self):
    self.cursor.execute("SELECT * FROM TagList")
    return self.cursor.fetchall()

  def update_row(self, target_name, target_update, reference_name, reference_content):
    # update targeted row `target_name, target_update` based on reference `reference_name, reference_content`
    self.cursor.execute("UPDATE TagList SET (?) = (?) WHERE ? = ?",
                        (target_name, target_update, reference_name, reference_content, ))
    self.connection.commit()

  def update_multi_rows(self, target_name, target_update, reference_name, reference_content):
    # update targeted row `target_name, target_update` based on reference `reference_name, reference_content`
    # used to update a large amount of rows

    for tn, tu, rf, rc in zip(target_name, target_update, reference_name, reference_content):
      self.cursor.execute("UPDATE TagList SET (?) = (?) WHERE ? = ?",
                          (tn, tu, rf, rc, ))

    self.connection.commit()



if __name__ == '__main__':
  LC = LogixController()
  for t in LC.TagList:
    print(t.TagName, t.DataType)

