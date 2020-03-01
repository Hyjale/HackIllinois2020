import numpy as np
from decimal import *
import matplotlib.pyplot as plt
import seaborn as sb
import pandas as pd
from pandas import *
from matplotlib import cm
from matplotlib import pyplot
from scipy.ndimage.filters import gaussian_filter
import matplotlib.image as mpimg
import types

def index_2d(myList, v):
    for i, x in enumerate(myList):
        if v in x:
            return i


posData = pd.read_csv('PositionTracker.csv')
posCons = pd.read_csv('mapSize.csv')
intData = pd.read_csv('objectInteractionTime.csv')
gazeData = pd.read_csv('Gaze.csv')
mapImg = mpimg.imread('map.png')

negX = posCons.iloc[0,0]    #save negX value
negZ = posCons.iloc[0,2]    #save negZ value
posX = posCons.iloc[0,1]    #save posX value
posZ = posCons.iloc[0,3]    #save posZ value
xPos = posData['XPos'].tolist()
zPos = posData['ZPos'].tolist()

posValGrab = []
grabTimeDurationLeft = []
grabTimeDurationRight = []
pointTimeDurationLeft = []
pointTimeDurationRight = []
colorArray = []

# Data extract from the objectInteractionTime.csv
for i in range(len(intData)):
    if intData.iloc[i,0] not in (i[0] for i in colorArray):
        colorArray.append([intData.iloc[i,0], plt.rcParams['axes.prop_cycle'].by_key()['color'][len(colorArray)%10]])
    if intData.iloc[i,4] or intData.iloc[i,6]:
        posValGrab.append([intData.iloc[i,0], [intData.iloc[i,10], intData.iloc[i,11]], [intData.iloc[i,14], intData.iloc[i,15]]])
        if intData.iloc[i,4]:
            grabTimeDurationLeft.append([intData.iloc[i,0], intData.iloc[i,8], intData.iloc[i,9], colorArray[index_2d(colorArray, intData.iloc[i,0])][1]])
        if intData.iloc[i,6]:
            grabTimeDurationRight.append([intData.iloc[i,0], intData.iloc[i,8], intData.iloc[i,9], colorArray[index_2d(colorArray, intData.iloc[i,0])][1]])
    if intData.iloc[i,1]:
        pointTimeDurationLeft.append([intData.iloc[i,1], intData.iloc[i,8], intData.iloc[i,9], colorArray[index_2d(colorArray, intData.iloc[i,0])][1]])
    if intData.iloc[i,2]:
        pointTimeDurationRight.append([intData.iloc[i,2], intData.iloc[i,8], intData.iloc[i,9], colorArray[index_2d(colorArray, intData.iloc[i,0])][1]])



#Draw Heatmap
xPos = [x - negX for x in xPos]     #position scaling: upper left to (0,0)
zPos = [x - negZ for x in zPos]     #position scaling: upper left to (0,0)
pos = [list(a) for a in zip(xPos, zPos)]
posMap = [[0 for i in range((posZ-negZ))] for j in range((posX-negX))]  #position map. 1 grid = 0.5 by 0.5
for i in range(len(pos)):
    posMap[int(pos[i][0])][int(pos[i][1])] += 1

pm_smooth = gaussian_filter(posMap, sigma=3)        #apply gaussian blur to the data
hmap = sb.heatmap(pm_smooth, vmin=np.min(pm_smooth), vmax=np.max(pm_smooth), cmap = "YlOrRd", alpha = 0.9, zorder = 2, cbar = False, square = True, xticklabels = False, yticklabels = False)
hmap.imshow(mapImg, aspect = hmap.get_aspect(), extent = hmap.get_xlim() + hmap.get_ylim(), zorder = 1)
plt.title('User Heatmap')
plt.savefig('AnalysisResult/heatmap.png', dpi = 300)
# plt.show()

# Draw Table(Most Gazed)
objTimeDict = []
# add interacted items to the list
for i in range(len(gazeData)):
    if gazeData.iloc[i,1] not in objTimeDict:
        objTimeDict.append(gazeData.iloc[i,1])
    if gazeData.iloc[i,1] not in (i[0] for i in colorArray):
        colorArray.append([gazeData.iloc[i,1], plt.rcParams['axes.prop_cycle'].by_key()['color'][len(colorArray)%10]])
# Create dictionary of object and gazed time
zero = [0]*len(objTimeDict)
objTimeDict = [list(a) for a in zip(objTimeDict, zero)]
for i in range(len(gazeData)):
    for j in range(len(objTimeDict)):
        if gazeData.iloc[i,1] is objTimeDict[j][0]:
            objTimeDict[j][1] += (gazeData.iloc[i,3] - gazeData.iloc[i,2])

objTimeDict.sort(key=lambda l:l[1], reverse = True)
for i in range(len(objTimeDict)):
    objTimeDict[i][1] = round(objTimeDict[i][1], 2)
fig, ax = plt.subplots()
# hide axes
fig.patch.set_visible(False)
ax.axis('off')
ax.axis('tight')

cellCols = []
for i in range(len(objTimeDict)):
    cellCols.append(["#FFFFFF", "#FFFFFF"])
for i in range(len(cellCols)):
    cellCols[i][0] = colorArray[index_2d(colorArray, objTimeDict[i][0])][1]

ax.table(cellText=objTimeDict, cellColours = cellCols, cellLoc = 'center', colLabels=("Interacted Objects", "Total Interacted Time(sec)"), loc = 'center')
fig.tight_layout()
plt.title('Total gaze time by objects')
plt.savefig('AnalysisResult/gaze.png', dpi = 300)




# position scaling
for i in range(len(posValGrab)):
    posValGrab[i][1] = [x - negX for x in posValGrab[i][1]]
    posValGrab[i][2] = [x - negZ for x in posValGrab[i][2]]

# Draw object trajectory map
ax = plt.axes()
x = range(300)
ax.imshow(mapImg, zorder=0, extent=[0, posX-negX, 0, posZ-negZ])
for i in range(len(posValGrab)):
    tempColor = colorArray[0][1]
    for j in range(len(colorArray)):
        if posValGrab[i][0] is colorArray[j][0]:
            tempColor = colorArray[j][1]
            break
    ax.plot(posValGrab[i][2], posValGrab[i][1], color = tempColor, zorder = 1)
ax.axis('off')
plt.title('Object trajectory map')
plt.savefig('AnalysisResult/trajectory.png', dpi = 300)

# Draw grab duration chart

figDCL, ax = plt.subplots()
for i in range(len(grabTimeDurationLeft)):
    ax.plot([grabTimeDurationLeft[i][1], grabTimeDurationLeft[i][2]], [0, 0], color = grabTimeDurationLeft[i][3], linewidth = 20)
plt.ylim(-.1, 0.1)
figDCL.tight_layout()
ax.yaxis.set_visible(False)
ax.spines['right'].set_visible(False)
ax.spines['top'].set_visible(False)
ax.spines['bottom'].set_visible(False)
plt.title('Object grabbed using left hand by time')
plt.savefig('AnalysisResult/grabLeft.png', dpi = 300)

figDCR, ax = plt.subplots()
for i in range(len(grabTimeDurationRight)):
    ax.plot([grabTimeDurationRight[i][1], grabTimeDurationRight[i][2]], [0, 0], color = grabTimeDurationRight[i][3], linewidth = 20)
plt.ylim(-.1, 0.1)
figDCR.tight_layout()
ax.yaxis.set_visible(False)
ax.spines['right'].set_visible(False)
ax.spines['top'].set_visible(False)
ax.spines['bottom'].set_visible(False)
plt.title('Object grabbed using right hand by time')
plt.savefig('AnalysisResult/grabRight.png', dpi = 300)

# Draw point duration chart

figDPL, ax = plt.subplots()
for i in range(len(pointTimeDurationLeft)):
    ax.plot([pointTimeDurationLeft[i][1], pointTimeDurationLeft[i][2]], [0, 0], color = pointTimeDurationLeft[i][3], linewidth = 20)
plt.ylim(-.1, 0.1)
figDPL.tight_layout()
ax.yaxis.set_visible(False)
ax.spines['right'].set_visible(False)
ax.spines['top'].set_visible(False)
ax.spines['bottom'].set_visible(False)
plt.title('Object pointed using left hand by time')
plt.savefig('AnalysisResult/pointLeft.png', dpi = 300)

figDPR, ax = plt.subplots()
for i in range(len(pointTimeDurationRight)):
    ax.plot([pointTimeDurationRight[i][1], pointTimeDurationRight[i][2]], [0, 0], color = pointTimeDurationRight[i][3], linewidth = 20)
plt.ylim(-.1, 0.1)
figDPR.tight_layout()
ax.yaxis.set_visible(False)
ax.spines['right'].set_visible(False)
ax.spines['top'].set_visible(False)
ax.spines['bottom'].set_visible(False)
plt.title('Object pointed using right hand by time')
plt.savefig('AnalysisResult/pointRight.png', dpi = 300)
