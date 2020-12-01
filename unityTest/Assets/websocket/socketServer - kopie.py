import socket
import json
import time

import random

y = True
while y == True:
    receiveFromClient = input("ontvang van simulator? y/n \n")
    if receiveFromClient != "y":
        if receiveFromClient != "n":
            print("verkeerde input, probeer opnieuw.")
        else:
            y = False
    else:
        y = False

sleepTime = int(input("delay tussen messages? \n 1 is 1 seconde \n"))

def formatHeader(y):
    x = None
    x = str(len(str(y))) + ":" + str(y)
    x.replace(" ", "")
    return x

with open('jason_controller.json') as file:
    jason = json.load(file)

jsonSimulator = None

host = '127.0.0.1'
port = 54000

def receiveFromSimulator():
    #car
    Group01A_array = ["A1-1", "A1-2", "A3-1", "A3-2", "A3-3", "A3-4"]
    Group01B_array = ["A1-1", "A1-2", "A2-1", "A2-2", "A3-3", "A4-4"]
    Group01C_array = ["A1-1", "A1-2", "A1-3", "A2-1", "A2-2"]
    Group02A_array = ["A2-1", "A2-2", "A2-3", "A2-4", "A3-3", "A3-4"]
    Group02B_array = ["A2-1", "A2-2", "A1-1", "A1-2", "A3-3", "A3-4"]

    #fiets + voet
    GroupFV01_array = ["A2-3", "A2-4", "A3-3", "A3-4", "F1-1", "F1-2", "V1-1", "V1-2", "V1-3", "V1-4"]
    GroupFV02_array = ["A1-1", "A1-2", "A3-1", "A3-2", "F2-1", "F2-2", "V2-1", "V2-2", "V2-3", "V2-4"]
    
    #bus
    GroupB01A_array = ["A3-1", "A3-2", "A3-3", "A3-4", "B1-1"]
    GroupB01B_array = ["A2-1", "A2-2", "A3-3", "A3-4", "B1-1"]
    GroupB02_array = ["A1-1", "A1-2", "A2-1", "A2-2", "B1-2"]

    
    TrafficLightsThatHasCarsEast = []


    for x in jsonSimulator :
        if(jsonSimulator[x] == 1):
            print(x)
            TrafficLightsThatHasCarsEast.append(x)
            
    resultsEast = []
    amountOfTrafficlightMatchesEast = []
    
    for i in range(10):
        resultsEast.append([])
        amountOfTrafficlightMatchesEast.append([])

    for i in TrafficLightsThatHasCarsEast:
        resultsEast[0].append(Group01A_array.count(i))
        resultsEast[1].append(Group01B_array.count(i))
        resultsEast[2].append(Group01C_array.count(i))
        resultsEast[3].append(Group02A_array.count(i))
        resultsEast[4].append(Group02B_array.count(i))
        resultsEast[5].append(GroupFV01_array.count(i))
        resultsEast[6].append(GroupFV02_array.count(i))
        resultsEast[7].append(GroupB01A_array.count(i))
        resultsEast[8].append(GroupB01B_array.count(i))
        resultsEast[9].append(GroupB02_array.count(i))
    
    amountOfTrafficlightMatchesEast[0].append("groep01A")
    amountOfTrafficlightMatchesEast[1].append("groep01B")
    amountOfTrafficlightMatchesEast[2].append("groep01C")
    amountOfTrafficlightMatchesEast[3].append("groep02A")
    amountOfTrafficlightMatchesEast[4].append("groep02B")
    amountOfTrafficlightMatchesEast[5].append("groepFV01")
    amountOfTrafficlightMatchesEast[6].append("groepFV02")
    amountOfTrafficlightMatchesEast[7].append("groepB01A")
    amountOfTrafficlightMatchesEast[8].append("groepB01B")
    amountOfTrafficlightMatchesEast[9].append("groepB02")
    
    for i in range(10):
        count = 0
        for j in resultsEast[i]:
            if(j == 1):
                count += 1
        amountOfTrafficlightMatchesEast[i].append(count)

    highestAmountOfMatchesGroupNameEast = ""
    highestAmountOfMatchesNumberEast = 0
    for i in range(len(amountOfTrafficlightMatchesEast)):
        if(amountOfTrafficlightMatchesEast[i][1] > highestAmountOfMatchesNumberEast):
            highestAmountOfMatchesNumberEast = amountOfTrafficlightMatchesEast[i][1]
            highestAmountOfMatchesGroupNameEast = amountOfTrafficlightMatchesEast[i][0]

    print(highestAmountOfMatchesGroupNameEast)
    groepAllRed()
    if(highestAmountOfMatchesGroupNameEast == "groep01A"):
        Group01A()
    elif(highestAmountOfMatchesGroupNameEast == "groep01B"):
        Group01B()
    elif(highestAmountOfMatchesGroupNameEast == "groep01C"):
        Group01C()
    elif(highestAmountOfMatchesGroupNameEast == "groep02A"):
        Group02A()
    elif(highestAmountOfMatchesGroupNameEast == "groep02B"):
        Group02B()
    elif(highestAmountOfMatchesGroupNameEast == "groepFV01"):
        GroupFV01()
    elif(highestAmountOfMatchesGroupNameEast == "groepFV02"):
        GroupFV02()
    elif(highestAmountOfMatchesGroupNameEast == "groepB01A"):
        GroupB01A()
    elif(highestAmountOfMatchesGroupNameEast == "groepB01B"):
        GroupB01B()
    elif(highestAmountOfMatchesGroupNameEast == "groepB02"):
        GroupB02()
    else:
        print("nothing")


def Group01A():
    jason["A1-1"] = 1
    jason["A1-2"] = 1
    jason["A3-1"] = 1
    jason["A3-2"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1

def Group01B():
    jason["A1-1"] = 1
    jason["A1-2"] = 1
    jason["A2-1"] = 1
    jason["A2-2"] = 1
    jason["A3-3"] = 1
    jason["A4-4"] = 1

def Group01C():
    jason["A1-1"] = 1
    jason["A1-2"] = 1
    jason["A1-3"] = 1
    jason["A2-1"] = 1
    jason["A2-2"] = 1

def Group02A():
    jason["A2-1"] = 1
    jason["A2-2"] = 1
    jason["A2-3"] = 1
    jason["A2-4"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1

def Group02B():
    jason["A2-1"] = 1
    jason["A2-2"] = 1
    jason["A1-1"] = 1
    jason["A1-2"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1
    
def GroupFV01():
    jason["A2-3"] = 1
    jason["A2-4"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1
    jason["F1-1"] = 1
    jason["F1-2"] = 1
    jason["V1-1"] = 1
    jason["V1-2"] = 1
    jason["V1-3"] = 1
    jason["V1-4"] = 1

def GroupFV02():
    jason["A1-1"] = 1
    jason["A1-2"] = 1
    jason["A3-1"] = 1
    jason["A3-2"] = 1
    jason["F2-1"] = 1
    jason["F2-2"] = 1
    jason["V2-1"] = 1
    jason["V2-2"] = 1
    jason["V2-3"] = 1
    jason["V2-4"] = 1

def GroupB01A():
    jason["A3-1"] = 1
    jason["A3-2"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1
    jason["B1-1"] = 1

def GroupB01B():
    jason["A2-1"] = 1
    jason["A2-2"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1
    jason["B1-1"] = 1

def GroupB02():
    jason["A1-1"] = 1
    jason["A1-2"] = 1
    jason["A2-1"] = 1
    jason["B1-2"] = 1
    
def changeToSame(x):
    jason["A1-1"] = x
    jason["A1-2"] = x
    jason["A1-3"] = x
    jason["A2-1"] = x
    jason["A2-2"] = x
    jason["A2-3"] = x
    jason["A2-4"] = x
    jason["A3-1"] = x
    jason["A3-2"] = x
    jason["A3-3"] = x
    jason["A3-4"] = x
    jason["A4-1"] = x
    jason["A4-2"] = x
    jason["A4-3"] = x
    jason["A4-4"] = x
    jason["A5-1"] = x
    jason["A5-2"] = x
    jason["A5-3"] = x
    jason["A5-4"] = x
    jason["A6-1"] = x
    jason["A6-2"] = x
    jason["A6-3"] = x
    jason["A6-4"] = x

def groepAllRed():
    #car
    jason["A1-1"] = 0
    jason["A1-2"] = 0
    jason["A1-3"] = 0
    jason["A2-1"] = 0
    jason["A2-2"] = 0
    jason["A2-3"] = 0
    jason["A2-4"] = 0
    jason["A3-1"] = 0
    jason["A3-2"] = 0
    jason["A3-3"] = 0
    jason["A3-4"] = 0
    jason["A4-1"] = 0
    jason["A4-2"] = 0
    jason["A4-3"] = 0
    jason["A4-4"] = 0
    jason["A5-1"] = 0
    jason["A5-2"] = 0
    jason["A5-3"] = 0
    jason["A5-4"] = 0
    jason["A6-1"] = 0
    jason["A6-2"] = 0
    jason["A6-3"] = 0
    jason["A6-4"] = 0

    #fiets + voet
    jason["F1-1"] = 0
    jason["F1-2"] = 0
    jason["F2-1"] = 0
    jason["F2-2"] = 0
    jason["F4-1"] = 0
    jason["F4-2"] = 0
    jason["F5-1"] = 0
    jason["F5-2"] = 0
    
    #bus
    jason["B1-1"] = 0
    jason["B1-2"] = 0
    jason["B4-1"] = 0
    
with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as sock:
    sock.bind((host, port))
    sock.listen()
    print("listening...")
    conn, addr = sock.accept()
    with conn:
        print('Connected by: ', addr)
        while True:
            if receiveFromClient == "y":
                data = conn.recv(1024)
                data = data.decode("utf-8")
                splittedData = data.split(":", 1)
                if (len(splittedData[1]) == int(splittedData[0])):
                    jsonSimulator = json.loads(splittedData[1])
                    
                    receiveFromSimulator()

                    z = json.dumps(jason)

                    message = formatHeader(z)

                    conn.sendall(message.encode("utf-8"))

                    time.sleep(sleepTime)
            else:

                groepAllRed()
                z = json.dumps(jason)
                message = formatHeader(z)
                conn.sendall(message.encode("utf-8"))
                time.sleep(sleepTime)
                print("0")
#
#                groepO1A()
#                z = json.dumps(jason)
#                message = formatHeader(z)
#                conn.sendall(message.encode("utf-8"))
#                time.sleep(sleepTime)
#                print("1")
#
#                groepO1B()
#                z = json.dumps(jason)
#                message = formatHeader(z)
#                conn.sendall(message.encode("utf-8"))
#                time.sleep(sleepTime)
#                print("2")
#
#                groepO1C()
#                z = json.dumps(jason)
#                message = formatHeader(z)
#                conn.sendall(message.encode("utf-8"))
#                time.sleep(sleepTime)
#                print("3")
#
#                groepO3A()
#                z = json.dumps(jason)
#                message = formatHeader(z)
#                conn.sendall(message.encode("utf-8"))
#                time.sleep(sleepTime)
#                print("4")
#
#                groepO3C()
#                z = json.dumps(jason)
#                message = formatHeader(z)
#                conn.sendall(message.encode("utf-8"))
#                time.sleep(sleepTime)
#                print("5")
#
#                groepW1A()
#                z = json.dumps(jason)
#                message = formatHeader(z)
#                conn.sendall(message.encode("utf-8"))
#                time.sleep(sleepTime)
#                print("6")
#
#                groepW2A()
#                z = json.dumps(jason)
#                message = formatHeader(z)
#                conn.sendall(message.encode("utf-8"))
#                time.sleep(sleepTime)
#                print("7")
#
#                groepW2B()
#                z = json.dumps(jason)
#                message = formatHeader(z)
#                conn.sendall(message.encode("utf-8"))
#                time.sleep(sleepTime)
#                print("8")
#
#                groepW3A()
#                z = json.dumps(jason)
#                message = formatHeader(z)
#                conn.sendall(message.encode("utf-8"))
#                time.sleep(sleepTime)
#                print("9")
#
