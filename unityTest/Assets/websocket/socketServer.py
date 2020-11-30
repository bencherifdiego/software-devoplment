import socket
import json
import time

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
    x = str(len(str(y))) + ":" + str(y)
    x.replace(" ", "")
    return x

with open('jason_controller.json') as file:
    jason = json.load(file)

host = '127.0.0.1'
port = 54000

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
    jason["A1-1"] = "0"
    jason["A1-2"] = "0"
    jason["A1-3"] = "0"
    jason["A2-1"] = "0"
    jason["A2-2"] = "0"
    jason["A2-3"] = "0"
    jason["A2-4"] = "0"
    jason["A3-1"] = "0"
    jason["A3-2"] = "0"
    jason["A3-3"] = "0"
    jason["A3-4"] = "0"
    jason["A4-1"] = "0"
    jason["A4-2"] = "0"
    jason["A4-3"] = "0"
    jason["A4-4"] = "0"
    jason["A5-1"] = "0"
    jason["A5-2"] = "0"
    jason["A5-3"] = "0"
    jason["A5-4"] = "0"
    jason["A6-1"] = "0"
    jason["A6-2"] = "0"
    jason["A6-3"] = "0"
    jason["A6-4"] = "0"

def groepO1A1():
    jason["A1-1"] = "1"
    jason["A1-2"] = "1"
    jason["A1-3"] = "0"
    jason["A2-1"] = "0"
    jason["A2-2"] = "0"
    jason["A2-3"] = "0"
    jason["A2-4"] = "0"
    jason["A3-1"] = "1"
    jason["A3-2"] = "1"
    jason["A3-3"] = "1"
    jason["A3-4"] = "1"
    jason["A4-1"] = "0"
    jason["A4-2"] = "0"
    jason["A4-3"] = "0"
    jason["A4-4"] = "0"
    jason["A5-1"] = "0"
    jason["A5-2"] = "0"
    jason["A5-3"] = "0"
    jason["A5-4"] = "0"
    jason["A6-1"] = "0"
    jason["A6-2"] = "0"
    jason["A6-3"] = "0"
    jason["A6-4"] = "0"

def groepO1B1():
    jason["A1-1"] = "1"
    jason["A1-2"] = "1"
    jason["A1-3"] = "0"
    jason["A2-1"] = "1"
    jason["A2-2"] = "1"
    jason["A2-3"] = "0"
    jason["A2-4"] = "0"
    jason["A3-1"] = "0"
    jason["A3-2"] = "0"
    jason["A3-3"] = "1"
    jason["A3-4"] = "1"
    jason["A4-1"] = "0"
    jason["A4-2"] = "0"
    jason["A4-3"] = "0"
    jason["A4-4"] = "0"
    jason["A5-1"] = "0"
    jason["A5-2"] = "0"
    jason["A5-3"] = "0"
    jason["A5-4"] = "0"
    jason["A6-1"] = "0"
    jason["A6-2"] = "0"
    jason["A6-3"] = "0"
    jason["A6-4"] = "0"

def groepO1C1():
    jason["A1-1"] = "1"
    jason["A1-2"] = "1"
    jason["A1-3"] = "1"
    jason["A2-1"] = "1"
    jason["A2-2"] = "1"
    jason["A2-3"] = "0"
    jason["A2-4"] = "0"
    jason["A3-1"] = "0"
    jason["A3-2"] = "0"
    jason["A3-3"] = "0"
    jason["A3-4"] = "0"
    jason["A4-1"] = "0"
    jason["A4-2"] = "0"
    jason["A4-3"] = "0"
    jason["A4-4"] = "0"
    jason["A5-1"] = "0"
    jason["A5-2"] = "0"
    jason["A5-3"] = "0"
    jason["A5-4"] = "0"
    jason["A6-1"] = "0"
    jason["A6-2"] = "0"
    jason["A6-3"] = "0"
    jason["A6-4"] = "0"

def groepO3A1():
    jason["A1-1"] = "0"
    jason["A1-2"] = "0"
    jason["A1-3"] = "0"
    jason["A2-1"] = "1"
    jason["A2-2"] = "1"
    jason["A2-3"] = "1"
    jason["A2-4"] = "1"
    jason["A3-1"] = "0"
    jason["A3-2"] = "0"
    jason["A3-3"] = "1"
    jason["A3-4"] = "1"
    jason["A4-1"] = "0"
    jason["A4-2"] = "0"
    jason["A4-3"] = "0"
    jason["A4-4"] = "0"
    jason["A5-1"] = "0"
    jason["A5-2"] = "0"
    jason["A5-3"] = "0"
    jason["A5-4"] = "0"
    jason["A6-1"] = "0"
    jason["A6-2"] = "0"
    jason["A6-3"] = "0"
    jason["A6-4"] = "0"

def groepO3C1():
    jason["A1-1"] = "1"
    jason["A1-2"] = "1"
    jason["A1-3"] = "0"
    jason["A2-1"] = "1"
    jason["A2-2"] = "1"
    jason["A2-3"] = "0"
    jason["A2-4"] = "0"
    jason["A3-1"] = "0"
    jason["A3-2"] = "0"
    jason["A3-3"] = "1"
    jason["A3-4"] = "1"
    jason["A4-1"] = "0"
    jason["A4-2"] = "0"
    jason["A4-3"] = "0"
    jason["A4-4"] = "0"
    jason["A5-1"] = "0"
    jason["A5-2"] = "0"
    jason["A5-3"] = "0"
    jason["A5-4"] = "0"
    jason["A6-1"] = "0"
    jason["A6-2"] = "0"
    jason["A6-3"] = "0"
    jason["A6-4"] = "0"

def groepW1A1():
    jason["A1-1"] = "0"
    jason["A1-2"] = "0"
    jason["A1-3"] = "0"
    jason["A2-1"] = "0"
    jason["A2-2"] = "0"
    jason["A2-3"] = "0"
    jason["A2-4"] = "0"
    jason["A3-1"] = "0"
    jason["A3-2"] = "0"
    jason["A3-3"] = "0"
    jason["A3-4"] = "0"
    jason["A4-1"] = "1"
    jason["A4-2"] = "1"
    jason["A4-3"] = "1"
    jason["A4-4"] = "1"
    jason["A5-1"] = "0"
    jason["A5-2"] = "0"
    jason["A5-3"] = "1"
    jason["A5-4"] = "1"
    jason["A6-1"] = "0"
    jason["A6-2"] = "0"
    jason["A6-3"] = "0"
    jason["A6-4"] = "0"

def groepW2A1():
    jason["A1-1"] = "0"
    jason["A1-2"] = "0"
    jason["A1-3"] = "0"
    jason["A2-1"] = "0"
    jason["A2-2"] = "0"
    jason["A2-3"] = "0"
    jason["A2-4"] = "0"
    jason["A3-1"] = "0"
    jason["A3-2"] = "0"
    jason["A3-3"] = "0"
    jason["A3-4"] = "0"
    jason["A4-1"] = "0"
    jason["A4-2"] = "0"
    jason["A4-3"] = "1"
    jason["A4-4"] = "1"
    jason["A5-1"] = "0"
    jason["A5-2"] = "0"
    jason["A5-3"] = "1"
    jason["A5-4"] = "1"
    jason["A6-1"] = "1"
    jason["A6-2"] = "1"
    jason["A6-3"] = "0"
    jason["A6-4"] = "0"

def groepW2B1():
    jason["A1-1"] = "0"
    jason["A1-2"] = "0"
    jason["A1-3"] = "0"
    jason["A2-1"] = "0"
    jason["A2-2"] = "0"
    jason["A2-3"] = "0"
    jason["A2-4"] = "0"
    jason["A3-1"] = "0"
    jason["A3-2"] = "0"
    jason["A3-3"] = "0"
    jason["A3-4"] = "0"
    jason["A4-1"] = "0"
    jason["A4-2"] = "0"
    jason["A4-3"] = "1"
    jason["A4-4"] = "1"
    jason["A5-1"] = "0"
    jason["A5-2"] = "0"
    jason["A5-3"] = "0"
    jason["A5-4"] = "0"
    jason["A6-1"] = "1"
    jason["A6-2"] = "1"
    jason["A6-3"] = "1"
    jason["A6-4"] = "1"

def groepW3A1():
    jason["A1-1"] = "0"
    jason["A1-2"] = "0"
    jason["A1-3"] = "0"
    jason["A2-1"] = "0"
    jason["A2-2"] = "0"
    jason["A2-3"] = "0"
    jason["A2-4"] = "0"
    jason["A3-1"] = "0"
    jason["A3-2"] = "0"
    jason["A3-3"] = "0"
    jason["A3-4"] = "0"
    jason["A4-1"] = "0"
    jason["A4-2"] = "0"
    jason["A4-3"] = "0"
    jason["A4-4"] = "0"
    jason["A5-1"] = "1"
    jason["A5-2"] = "1"
    jason["A5-3"] = "1"
    jason["A5-4"] = "1"
    jason["A6-1"] = "1"
    jason["A6-2"] = "1"
    jason["A6-3"] = "0"
    jason["A6-4"] = "0"

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as sock:
    sock.bind((host, port))
    sock.listen()
    conn, addr = sock.accept()
    with conn:
        print('Connected by: ', addr)
        while True:
            if receiveFromClient == "y":
                data = conn.recv(1024)
                data = data.decode("utf-8")
                splittedData = data.split(":", 1)
                if (len(splittedData[1]) == int(splittedData[0])):
                    x = json.loads(splittedData[1])
                    print(splittedData[1])
                    print(x["A1-1"])
                    if (x["A1-1"] == '0'):
                        changeToSame(0)
                    elif (x["A1-1"] == '1'):
                        changeToSame(1)
                    z = json.dumps(jason)

                    message = formatHeader(z)

                    conn.sendall(message.encode("utf-8"))
            else:

                groepAllRed()
                z = json.dumps(jason)
                message = formatHeader(z)
                conn.sendall(message.encode("utf-8"))
                time.sleep(sleepTime)
                print("0")

                groepO1A()
                z = json.dumps(jason)
                message = formatHeader(z)
                conn.sendall(message.encode("utf-8"))
                time.sleep(sleepTime)
                print("1")

                groepO1B()
                z = json.dumps(jason)
                message = formatHeader(z)
                conn.sendall(message.encode("utf-8"))
                time.sleep(sleepTime)
                print("2")

                groepO1C()
                z = json.dumps(jason)
                message = formatHeader(z)
                conn.sendall(message.encode("utf-8"))
                time.sleep(sleepTime)
                print("3")

                groepO3A()
                z = json.dumps(jason)
                message = formatHeader(z)
                conn.sendall(message.encode("utf-8"))
                time.sleep(sleepTime)
                print("4")

                groepO3C()
                z = json.dumps(jason)
                message = formatHeader(z)
                conn.sendall(message.encode("utf-8"))
                time.sleep(sleepTime)
                print("5")

                groepW1A()
                z = json.dumps(jason)
                message = formatHeader(z)
                conn.sendall(message.encode("utf-8"))
                time.sleep(sleepTime)
                print("6")

                groepW2A()
                z = json.dumps(jason)
                message = formatHeader(z)
                conn.sendall(message.encode("utf-8"))
                time.sleep(sleepTime)
                print("7")

                groepW2B()
                z = json.dumps(jason)
                message = formatHeader(z)
                conn.sendall(message.encode("utf-8"))
                time.sleep(sleepTime)
                print("8")

                groepW3A()
                z = json.dumps(jason)
                message = formatHeader(z)
                conn.sendall(message.encode("utf-8"))
                time.sleep(sleepTime)
                print("9")