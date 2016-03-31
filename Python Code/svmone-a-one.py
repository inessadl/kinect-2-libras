from sklearn import svm
from sklearn.svm import SVC
import csv

class SVMTraining(object):

	def __init__(self,p1):
		global trainSet
		global testSet
		global answerTrain
		global percentTrain
		global percentTest
		global lastIndex
		global answerTest
		percentTrain = p1
		percentTest = 1.0 - percentTrain
		trainSet = []
		answerTrain = []
		answerTest = []
		lastIndex = 0
		testSet = []

	def readingData(self,pathFile):
		readList = []
		with open(pathFile, 'rb') as f:
			reader = csv.reader(f)
			l = list(reader)
			readList = l

		#Carrega o conjunto de treinamento e as respostas esperadas
		for i in xrange(int(len(readList)*percentTrain)):
			gesture = []

			for k in xrange(len(readList[0])-1):
				gesture.append(float(readList[i][k]))
			
			answerTrain.append(float(readList[i][k+1]))
			trainSet.append(gesture)

		# Carrega o conjunto de teste e suas respectivas respostas
		for index in xrange(i+1,len(readList)):
			gestureTest = []	
			for k in xrange(len(readList[0])-1):
				gestureTest.append(float(readList[index][k]))
			
			answerTest.append(float(readList[index][k+1]))
			testSet.append(gestureTest)
		
	#Tem que corrigir
	def training(self):
		#clf = svm.SVC(decision_function_shape='ovo')
		clf = svm.LinearSVC()
		print answerTrain
		clf.fit(trainSet,answerTrain)

		
		print clf.predict([[332,110,361,125,298,166,298,162,312,120,335.0002,156.4871,333.5001,133.2436,351.5153,140.7436,319.6971,161.2436,316.5001,167.6248,320.2971,138.2436]])
        
        
        
        
        

x = SVMTraining(0.8)
x.readingData("fingers.csv")
#x.training()
