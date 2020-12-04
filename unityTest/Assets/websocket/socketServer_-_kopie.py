import socket
import json
import time
import threading    
import random

#Ask for delay time between sending JSON from controller to simulator
sleepTime = int(input("delay tussen messages?\n1 is 1 seconde\n"))

#Checks how many characters are in the received JSON
def formatHeader(y):
    y = y.replace(" ", "")
    x = None
    x = str(len(str(y))) + ":" + str(y)
    x.replace(" ", "")
    return x

#Set empty JSON string of all the trafic lights
jason = '{"A1-1":0,"A1-2":0,"A1-3":0,"B1-1":0,"B1-2":0,"F1-1":0,"F1-2":0,"V1-1":0,"V1-2":0,"V1-3":0,"V1-4":0,"A2-1":0,"A2-2":0,"A2-3":0,"A2-4":0,"F2-1":0,"F2-2":0,"V2-1":0,"V2-2":0,"V2-3":0,"V2-4":0,"A3-1":0,"A3-2":0,"A3-3":0,"A3-4":0,"A4-1":0,"A4-2":0,"A4-3":0,"A4-4":0,"B4-1":0,"F4-1":0,"F4-2":0,"V4-1":0,"V4-2":0,"V4-3":0,"V4-4":0,"A5-1":0,"A5-2":0,"A5-3":0,"A5-4":0,"F5-1":0,"F5-2":0,"V5-1":0,"V5-2":0,"V5-3":0,"V5-4":0,"A6-1":0,"A6-2":0,"A6-3":0,"A6-4":0}'

#Decode the JSON string
jason = json.loads(jason)

#Set a seperate JSON string for the red traffic lights
jasonRed = '{"A1-1":0,"A1-2":0,"A1-3":0,"B1-1":0,"B1-2":0,"F1-1":0,"F1-2":0,"V1-1":0,"V1-2":0,"V1-3":0,"V1-4":0,"A2-1":0,"A2-2":0,"A2-3":0,"A2-4":0,"F2-1":0,"F2-2":0,"V2-1":0,"V2-2":0,"V2-3":0,"V2-4":0,"A3-1":0,"A3-2":0,"A3-3":0,"A3-4":0,"A4-1":0,"A4-2":0,"A4-3":0,"A4-4":0,"B4-1":0,"F4-1":0,"F4-2":0,"V4-1":0,"V4-2":0,"V4-3":0,"V4-4":0,"A5-1":0,"A5-2":0,"A5-3":0,"A5-4":0,"F5-1":0,"F5-2":0,"V5-1":0,"V5-2":0,"V5-3":0,"V5-4":0,"A6-1":0,"A6-2":0,"A6-3":0,"A6-4":0}'

host = '127.0.0.1'
port = 54000

#Declare two arrays in which the trafficlight(groups) are placed in when they have been called
arrayGroupHasBeenUsedEast = []
arrayGroupHasBeenUsedWest = []

#ReceiveFromSimulator function uses JSON data received from the simulator
#The function calculates which traffic lights should turn on and which should turn off.
def receiveFromSimulator(jsonSimulator):

    #Checks how many groups have been used, if there are more than 3 groups in the array; remove the first
    if(len(arrayGroupHasBeenUsedWest) > 3):
        arrayGroupHasBeenUsedWest.pop(0)

    if(len(arrayGroupHasBeenUsedEast) > 3):
        arrayGroupHasBeenUsedEast.pop(0)

    #Arrays of (car) traffic lights. These traffic lights can be turned on at the same time.
    Group01A_array = ["A1-1", "A1-2", "A3-1", "A3-2", "A3-3", "A3-4"]
    Group01B_array = ["A1-1", "A1-2", "A2-1", "A2-2", "A3-3", "A4-4"]
    Group01C_array = ["A1-1", "A1-2", "A1-3", "A2-1", "A2-2"]
    Group02A_array = ["A2-1", "A2-2", "A2-3", "A2-4", "A3-3", "A3-4"]
    Group02B_array = ["A2-1", "A2-2", "A1-1", "A1-2", "A3-3", "A3-4"]
    GroupW1A_array = ["A4-1", "A4-2", "A4-3", "A4-4", "A5-3", "A5-4"]
    GroupW2A_array = ["A4-3", "A4-4", "A5-3", "A5-4", "A6-1", "A6-2"]
    GroupW2B_array = ["A4-3", "A4-4", "A6-1", "A6-2", "A6-3", "A6-4"]
    GroupW3A_array = ["A5-1", "A5-2", "A5-3", "A5-4", "A6-1", "A6-2"]

    #Arrays of traffic lights with cyclists and pedestrian, combined with the cars. These traffic lights can be turned on at the same time.
    GroupFV01_array = ["A2-3", "A2-4", "A3-3", "A3-4", "F1-1", "F1-2", "V1-1", "V1-2", "V1-3", "V1-4"]
    GroupFV02_array = ["A1-1", "A1-2", "A3-1", "A3-2", "F2-1", "F2-2", "V2-1", "V2-2", "V2-3", "V2-4"]
    GroupFVW1_array = ["A5-1", "A5-2", "A6-1", "A6-2", "F4-1", "F4-2", "V4-1", "V4-2", "V4-3", "V4-4"]
    GroupFVW2_array = ["A4-3", "A4-3", "A6-3", "A6-4", "F5-1", "F5-2", "V5-1", "V5-2", "V5-3", "V5-4"]

    #Arrays of traffic lights with busses combined with the cars. These traffic lights can be turned on at the same time.
    GroupB01A_array = ["A3-1", "A3-2", "A3-3", "A3-4", "B1-1"]
    GroupB01B_array = ["A2-1", "A2-2", "A3-3", "A3-4", "B1-1"]
    GroupB02_array = ["A1-1", "A1-2", "A2-1", "A2-2", "B1-2"]
    GroupBW1A_array = ["A4-1", "A4-2", "A5-3", "A5-4", "B4-1"]
    GroupBW1B_array = ["A5-3", "A5-4", "A6-1", "A6-2", "B4-1"]
    GroupBW1C_array = ["A6-1", "A6-2", "A6-3", "A6-4", "B4-1"]
    
    #Declare an empty array
    TrafficLightsThatHasCars = []
    #Fill the array TrafficLightsThatHasCars with traffic lights (names ex: A4-1) that has cars/busses/cyclists or pedestrians waiting
    for x in jsonSimulator :
        if(jsonSimulator[x] == 1):
            TrafficLightsThatHasCars.append(x)

    #Declare arrays for east and west traffic lights, to check how many matches there are.
    resultsEast = []
    resultsWest = []

    #Declare arrays for counting the amount of matched cars waiting with the traffic lights certain group
    amountOfTrafficlightMatchesEast = []
    amountOfTrafficlightMatchesWest = []

    #Fill the arrays resultsEast+West and amountOfTrafficLightMatchesEast+West with new inner arrays for the amount of arrays declared above.
    for i in range(10):
        resultsEast.append([])
        amountOfTrafficlightMatchesEast.append([])

    for i in range(9):
        resultsWest.append([])
        amountOfTrafficlightMatchesWest.append([])
    
    #Loops through all the named traffic lights in the array TrafficLightsThatHasCars. 
    #Then check if the name in the group matches the one in the TrafficLightsThatHasCars array, if so place a 1 if not place a 0
    for i in TrafficLightsThatHasCars:
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
        resultsWest[0].append(GroupW1A_array.count(i))
        resultsWest[1].append(GroupW2A_array.count(i))
        resultsWest[2].append(GroupW2B_array.count(i))
        resultsWest[3].append(GroupW3A_array.count(i))
        resultsWest[4].append(GroupFVW1_array.count(i))
        resultsWest[5].append(GroupFVW2_array.count(i))
        resultsWest[6].append(GroupBW1A_array.count(i))
        resultsWest[7].append(GroupBW1B_array.count(i))
        resultsWest[8].append(GroupBW1C_array.count(i))

    #append the name of the group in the amountOfTrafficlightMatches arrays and count how many traffic lights are in that group
    amountOfTrafficlightMatchesEast[0].append("groep01A")
    amountOfTrafficlightMatchesEast[0].append(len(Group01A_array))
    amountOfTrafficlightMatchesEast[1].append("groep01B")
    amountOfTrafficlightMatchesEast[1].append(len(Group01B_array))
    amountOfTrafficlightMatchesEast[2].append("groep01C")
    amountOfTrafficlightMatchesEast[2].append(len(Group01C_array))
    amountOfTrafficlightMatchesEast[3].append("groep02A")
    amountOfTrafficlightMatchesEast[3].append(len(Group02A_array))
    amountOfTrafficlightMatchesEast[4].append("groep02B")
    amountOfTrafficlightMatchesEast[4].append(len(Group02B_array))
    amountOfTrafficlightMatchesEast[5].append("groepFV01")
    amountOfTrafficlightMatchesEast[5].append(len(GroupFV01_array))
    amountOfTrafficlightMatchesEast[6].append("groepFV02")
    amountOfTrafficlightMatchesEast[6].append(len(GroupFV02_array))
    amountOfTrafficlightMatchesEast[7].append("groepB01A")
    amountOfTrafficlightMatchesEast[7].append(len(GroupB01A_array))
    amountOfTrafficlightMatchesEast[8].append("groepB01B")
    amountOfTrafficlightMatchesEast[8].append(len(GroupB01B_array))
    amountOfTrafficlightMatchesEast[9].append("groepB02")
    amountOfTrafficlightMatchesEast[9].append(len(GroupB02_array))
    amountOfTrafficlightMatchesWest[0].append("groepW1A")
    amountOfTrafficlightMatchesWest[0].append(len(GroupW1A_array))
    amountOfTrafficlightMatchesWest[1].append("groepW2A")
    amountOfTrafficlightMatchesWest[1].append(len(GroupW2A_array))
    amountOfTrafficlightMatchesWest[2].append("groepW2B")
    amountOfTrafficlightMatchesWest[2].append(len(GroupW2B_array))
    amountOfTrafficlightMatchesWest[3].append("groepW3A")
    amountOfTrafficlightMatchesWest[3].append(len(GroupW3A_array))
    amountOfTrafficlightMatchesWest[4].append("groepFVW1")
    amountOfTrafficlightMatchesWest[4].append(len(GroupFVW1_array))
    amountOfTrafficlightMatchesWest[5].append("groepFVW2")
    amountOfTrafficlightMatchesWest[5].append(len(GroupFVW2_array))
    amountOfTrafficlightMatchesWest[6].append("groepBW1A")
    amountOfTrafficlightMatchesWest[6].append(len(GroupBW1A_array))
    amountOfTrafficlightMatchesWest[7].append("groepBW1B")
    amountOfTrafficlightMatchesWest[7].append(len(GroupBW1B_array))
    amountOfTrafficlightMatchesWest[8].append("groepBW1C")
    amountOfTrafficlightMatchesWest[8].append(len(GroupBW1C_array))

    #count how many times there is a match between the traffic lights that has cars waiting at them and the traffic lights specified in the group array
    for i in range(10):
        count = 0
        for j in resultsEast[i]:
            if(j == 1):
                count += 1
        amountOfTrafficlightMatchesEast[i].append(count)

    for i in range(9):
        count = 0
        for j in resultsWest[i]:
            if (j == 1):
                count += 1
        amountOfTrafficlightMatchesWest[i].append(count)

    #Set empty variables for choosing a group to set to green
    highestAmountOfMatchesGroupNameEast = ""
    highestAmountOfMatchesNumberEast = 0
    setBusEast = False

    highestAmountOfMatchesGroupNameWest = ""
    highestAmountOfMatchesNumberWest = 0
    setBusWest = False

    #set all traffic lights to red
    groepAllRed()
    
    busEastIsSet = False
    #loop through all the traffic lights in the JSON
    for s in jsonSimulator:
        if(busEastIsSet == False):
            if(s == "B1-1"):
                #If there is a bus waiting at traffic light B1-1 set that group to green
                if(jsonSimulator[s] > 0):
                    #Choose a group to set to green
                    randomChooseGroup = random.randint(0,1)
                    if(randomChooseGroup > 0):
                        GroupB01A()
                        setBusEast = True
                    else:
                        GroupB01B()
                        setBusEast = True
                    busEastIsSet = True
            elif(s == "B1-2"):
                #If there is a bus waiting at traffic light B1-2 set that group to green
                if(jsonSimulator[s] > 0):
                    GroupB02()
                    setBusEast = True
                    found = True
        if(s == "B4-1"):
            #If there is a bus waiting at traffic light B4-1 set that group to green
            if(jsonSimulator[s] > 0):
                #Choose a group to set to green
                randomChooseGroup = random.randint(0,2)
                if(randomChooseGroup == 0):
                    print(1)
                    GroupBW1A()
                    setBusWest = True
                elif(randomChooseGroup == 1):
                    print(2)
                    GroupBW1B()
                    setBusWest = True
                elif(randomChooseGroup == 2):
                    print(3)
                    GroupBW1C()
                    setBusWest = True

    #Declare two empty arrays. These arrays are filled with the groups that have the most amount of matches with where traffic is waiting at a traffic light
    PickOneEast = []
    PickOneWest = []

    #loop x amount of times. X is defined by the length of the amountOfTrafficlightMatchesEast array
    for i in range(len(amountOfTrafficlightMatchesEast)):
        #checks if amount of matches divided by the length of the amount of traffic lights in that group is greater then the highest amount of matches in east
        #and checks if the group hasn't been called already in the previous three times (checks in the arrayGroupHasBeenUsedEast array )
        if((amountOfTrafficlightMatchesEast[i][2] / amountOfTrafficlightMatchesEast[i][1]) > highestAmountOfMatchesNumberEast and amountOfTrafficlightMatchesEast[i][0] not in arrayGroupHasBeenUsedEast):
            #If the amount is higher set the highest amount to the amount of matches in the group divided by the amount of traffic lights in that group
            highestAmountOfMatchesNumberEast = (amountOfTrafficlightMatchesEast[i][2] / amountOfTrafficlightMatchesEast[i][1])
            #Set the name of the highest group to the current given group name
            highestAmountOfMatchesGroupNameEast = amountOfTrafficlightMatchesEast[i][0]
            PickOneEast = []
            #Append the name of the group to the PickOneEast array
            PickOneEast.append(highestAmountOfMatchesGroupNameEast)
        #Checks if the amount of matches divided by the length of the amount of traffic lights in that group is equal to the currect highest amount of matches in east
        elif((amountOfTrafficlightMatchesEast[i][2] / amountOfTrafficlightMatchesEast[i][1]) == highestAmountOfMatchesNumberEast and amountOfTrafficlightMatchesEast[i][0] not in arrayGroupHasBeenUsedEast):
            PickOneEast.append(amountOfTrafficlightMatchesEast[i][0])

    #Same as above but then for west
    for i in range(len(amountOfTrafficlightMatchesWest)):
        if((amountOfTrafficlightMatchesWest[i][2] / amountOfTrafficlightMatchesWest[i][1]) > highestAmountOfMatchesNumberWest and amountOfTrafficlightMatchesWest[i][0] not in arrayGroupHasBeenUsedWest):
            highestAmountOfMatchesNumberWest = (amountOfTrafficlightMatchesWest[i][2] / amountOfTrafficlightMatchesWest[i][1])
            highestAmountOfMatchesGroupNameWest = amountOfTrafficlightMatchesWest[i][0]
            PickOneWest = []
            PickOneWest.append(highestAmountOfMatchesGroupNameWest)
        elif((amountOfTrafficlightMatchesWest[i][2] / amountOfTrafficlightMatchesWest[i][1]) == highestAmountOfMatchesNumberWest and amountOfTrafficlightMatchesWest[i][0] not in arrayGroupHasBeenUsedWest):
            PickOneWest.append(amountOfTrafficlightMatchesEast[i][0])
    
    #Checks how many groups have an equal amount of traffic waiting divided by the amount of traffic lights in that group
    LenEast = len(PickOneEast)
    LenWest = len(PickOneWest)

    #If there are more then one group that have an equal amount of traffic waiting divided by the amount of traffic lights in that group, pick one group
    if(LenEast > 1):
        highestAmountOfMatchesGroupNameEast = PickOneEast[random.randint(0,(LenEast-1))]

    if(LenWest > 1):
        highestAmountOfMatchesGroupNameWest = PickOneWest[random.randint(0,(LenWest-1))]

    #If there is no bus waiting at east, call on the group with the highest amount of traffic waiting
    #In total there will be one group from east and one from west set to green
    if(setBusEast == False):
        arrayGroupHasBeenUsedEast.append(highestAmountOfMatchesGroupNameEast)
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

    #If there is no bus waiting at west, call on the group with the highest amount of traffic waiting
    #In total there will be one group from east and one from west set to green
    if(setBusWest == False):
        arrayGroupHasBeenUsedWest.append(highestAmountOfMatchesGroupNameWest)
        if(highestAmountOfMatchesGroupNameWest == "groepW1A"):
            GroupW1A()
        elif(highestAmountOfMatchesGroupNameWest == "groepW2A"):
            GroupW2A()
        elif(highestAmountOfMatchesGroupNameWest == "groepW2B"):
            GroupW2B()
        elif(highestAmountOfMatchesGroupNameWest == "groepW3A"):
            GroupW3A()
        elif (highestAmountOfMatchesGroupNameWest == "groepFVW1"):
            GroupFVW1()
        elif (highestAmountOfMatchesGroupNameWest == "groepFVW2"):
            GroupFVW2()
        elif (highestAmountOfMatchesGroupNameWest == "groepBW1A"):
            GroupBW1A()
        elif (highestAmountOfMatchesGroupNameWest == "groepBW1B"):
            GroupBW1B()
        elif (highestAmountOfMatchesGroupNameWest == "groepBW1C"):
            GroupBW1C()

#Set traffic lights in this group to green
def Group01A():
    jason["A1-1"] = 1
    jason["A1-2"] = 1
    jason["A3-1"] = 1
    jason["A3-2"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1

#Set traffic lights in this group to green
def Group01B():
    jason["A1-1"] = 1
    jason["A1-2"] = 1
    jason["A2-1"] = 1
    jason["A2-2"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1

#Set traffic lights in this group to green
def Group01C():
    jason["A1-1"] = 1
    jason["A1-2"] = 1
    jason["A1-3"] = 1
    jason["A2-1"] = 1
    jason["A2-2"] = 1

#Set traffic lights in this group to green
def Group02A():
    jason["A2-1"] = 1
    jason["A2-2"] = 1
    jason["A2-3"] = 1
    jason["A2-4"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1

#Set traffic lights in this group to green
def Group02B():
    jason["A2-1"] = 1
    jason["A2-2"] = 1
    jason["A1-1"] = 1
    jason["A1-2"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1

#Set traffic lights in this group to green
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

#Set traffic lights in this group to green
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

#Set traffic lights in this group to green
def GroupB01A():
    jason["A3-1"] = 1
    jason["A3-2"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1
    jason["B1-1"] = 1

#Set traffic lights in this group to green
def GroupB01B():
    jason["A2-1"] = 1
    jason["A2-2"] = 1
    jason["A3-3"] = 1
    jason["A3-4"] = 1
    jason["B1-1"] = 1

#Set traffic lights in this group to green
def GroupB02():
    jason["A1-1"] = 1
    jason["A1-2"] = 1
    jason["A2-1"] = 1
    jason["B1-2"] = 1

#Set traffic lights in this group to green
def GroupW1A():
    jason["A4-1"] = 1
    jason["A4-2"] = 1
    jason["A4-3"] = 1
    jason["A4-4"] = 1
    jason["A5-3"] = 1
    jason["A5-4"] = 1

#Set traffic lights in this group to green
def GroupW2A():
    jason["A4-3"] = 1
    jason["A4-4"] = 1
    jason["A5-3"] = 1
    jason["A5-4"] = 1
    jason["A6-1"] = 1
    jason["A6-2"] = 1

#Set traffic lights in this group to green
def GroupW2B():
    jason["A4-3"] = 1
    jason["A4-4"] = 1
    jason["A6-1"] = 1
    jason["A6-2"] = 1
    jason["A6-3"] = 1
    jason["A6-4"] = 1

#Set traffic lights in this group to green
def GroupW3A():
    jason["A5-1"] = 1
    jason["A5-2"] = 1
    jason["A5-3"] = 1
    jason["A5-4"] = 1
    jason["A6-1"] = 1
    jason["A6-2"] = 1

#Set traffic lights in this group to green
def GroupFVW1():
    jason["A5-1"] = 1
    jason["A5-2"] = 1
    jason["A6-1"] = 1
    jason["A6-2"] = 1
    jason["F4-1"] = 1
    jason["F4-2"] = 1
    jason["V4-1"] = 1
    jason["V4-2"] = 1
    jason["V4-3"] = 1
    jason["V4-4"] = 1

#Set traffic lights in this group to green
def GroupFVW2():
    jason["A4-3"] = 1
    jason["A4-4"] = 1
    jason["A6-3"] = 1
    jason["A6-4"] = 1
    jason["F5-1"] = 1
    jason["F5-2"] = 1
    jason["V5-1"] = 1
    jason["V5-2"] = 1
    jason["V5-3"] = 1
    jason["V5-4"] = 1

#Set traffic lights in this group to green
def GroupBW1A():
    jason["A4-1"] = 1
    jason["A4-2"] = 1
    jason["A5-3"] = 1
    jason["A5-4"] = 1
    jason["B4-1"] = 1

#Set traffic lights in this group to green
def GroupBW1B():
    jason["A5-3"] = 1
    jason["A5-4"] = 1
    jason["A6-1"] = 1
    jason["A6-2"] = 1
    jason["B4-1"] = 1

#Set traffic lights in this group to green
def GroupBW1C():
    jason["A6-1"] = 1
    jason["A6-2"] = 1
    jason["A6-3"] = 1
    jason["A6-4"] = 1
    jason["B4-1"] = 1

#Set all traffic lights to red
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
    
class myThread (threading.Thread):
    def __init__(self, threadID):
        threading.Thread.__init__(self)
        self.threadID = threadID
    def run(self):
        while True:
            data = conn.recv(1024)
            print(time.strftime("%H:%M:%S", time.localtime()), " : message received")
            data = data.decode("utf-8")
            splittedData = data.split(":", 1)
            if (len(splittedData[1]) == int(splittedData[0])):
                print(time.strftime("%H:%M:%S", time.localtime()), " : json correct length")
                jsonSimulator = json.loads(splittedData[1])
                if (jsonSimulator != None):
                    receiveFromSimulator(jsonSimulator)  
            else:
                print(time.strftime("%H:%M:%S", time.localtime()), " : json incorrect length")  

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as sock:
    sock.bind((host, port))
    sock.listen()
    print("listening...")
    conn, addr = sock.accept()
    with conn:
        print('Connected by: ', addr)
        thread1 = myThread(1)
        thread1.start()
        while True:
            Red = jasonRed
            messageRed = Red.replace("\\", "")
            messageRed = formatHeader(messageRed)
            conn.sendall(messageRed.encode("utf-8"))
            print("sent all red")
            time.sleep(5)

            z = json.dumps(jason)
            message = z.replace("\\", "")
            message = formatHeader(message)
            conn.sendall(message.encode("utf-8"))
            print(time.strftime("%H:%M:%S", time.localtime()), message)
            time.sleep(sleepTime)
