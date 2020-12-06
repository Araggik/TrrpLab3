import random
import datetime
from zeep import Client
from zeep.wsse.username import UsernameToken
from zeep.wsse.utils import WSU

#ws-security
timestamp_token = WSU.Timestamp()
today_datetime = datetime.datetime.today()
expires_datetime = today_datetime + datetime.timedelta(minutes=10)
timestamp_elements = [
        WSU.Created(today_datetime.strftime("%Y-%m-%dT%H:%M:%SZ")),
        WSU.Expires(expires_datetime.strftime("%Y-%m-%dT%H:%M:%SZ"))]
timestamp_token.extend(timestamp_elements)
id = str(random.randint(1,1000000))
user_name_token = UsernameToken('username'+id, 'password', timestamp_token=timestamp_token)

client = Client(
    'http://25.74.30.162:8080/our/service?singleWsdl', wsse=user_name_token)


#client = Client('http://25.74.30.162:8080/our/service?singleWsdl')

#Просмотр xml
#import logging.config

#logging.config.dictConfig({
#    'version': 1,
#    'formatters': {
#        'verbose': {
#            'format': '%(name)s: %(message)s'
#        }
#    },
#    'handlers': {
#        'console': {
#            'level': 'DEBUG',
#            'class': 'logging.StreamHandler',
#            'formatter': 'verbose',
#        },
#    },
#    'loggers': {
#        'zeep.transports': {
#            'level': 'DEBUG',
#            'propagate': True,
#            'handlers': ['console'],
#        },
#    }
#})

import sqlite3

count_send_columns = 5
count_columns = 13

message = input()
while message!="0":
  if message!="1":
   result = client.service.GetData(message)
   print(result)
   logging.debug("Help")
  else:
   conn = sqlite3.connect("Companies.db")
   cursor = conn.cursor()
   cursor.execute("SELECT * FROM Companies")
   rows = cursor.fetchall()
   for row in rows:
    i=1
    s = str(row[0])
    while i<count_columns:
     #columns = []
     #number =0
     #while number<count_send_columns and i<count_columns:
     #  columns.append(str(row[i]))
     #  i=i+1
     #  number=number+1
     #print(columns)
     #client.service.ReadColumns(columns) 
     s = s+"|"+str(row[i])
     i=i+1
    print(s)
    client.service.AppendDataInDB(s)
  message = input()