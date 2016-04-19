from sklearn import svm
from sklearn import tree
from sklearn.svm import SVC
from sklearn.linear_model import SGDClassifier
import csv
import numpy as np
import random
import sys

####################################
#Parametros
#
# argv[1] = pathFile
# argv[2] = Algoritmo de aprendizado (OVO,OVR)
# argv[3] = porcentagem de gestos para treinamento de 0 a 1
#
#Sample
#  python trainingGesture.py fingers.csv ovo 0.8
#####################################
class SVMTraining(object):

	def __init__(self,p1):
		global __trainSet,__testSet,__answerTrain,__percentTrain,__percentTest,lastIndex,__answerTest
		self.__percentTrain = p1
		self.__percentTest = 1.0 - self.__percentTrain
		self.__trainSet = []
		self.__answerTrain = []
		self.__answerTest = []
		lastIndex =  0
		self.__testSet = []

	def readingData(self,pathFile):
		readList = []
		print ("...load data set "+pathFile)
		with open(pathFile, 'rb') as f:
			reader = csv.reader(f)
			readList = list(reader)

		#Realiza a mistura do data set, para nunca ter o mesmo esquema de gestos
		print ("Total of Gesture: "+str(len(readList)))
		print ("... mixed dataSet")
		random.shuffle(readList)

		#Normaliza todos com o centro de cada mao

		#Carrega o conjunto de treinamento e as respostas esperadas
		print ("... making a training set")
		for i in xrange(int(len(readList)*self.__percentTrain)):
			gesture = []

			for k in xrange(len(readList[0])-1):
				if((k!=10)and(k!=11)):
					if(k%2==0):
						gesture.append(round((np.float64(readList[i][k])-np.float64(readList[i][10]))**2,2))
					else:
						gesture.append(round((np.float64(readList[i][k])-np.float64(readList[i][11]))**2,2))

			self.__answerTrain.append(np.float64(readList[i][k+1]))
			self.__trainSet.append(gesture)

			#print gesture
		print ("making a test set")
		# Carrega o conjunto de teste e suas respectivas respostas
		for index in xrange(i+1,len(readList)):
			gestureTest = []
			for k in xrange(len(readList[0])-1):
				if((k!=10)and(k!=11)):
					if(k%2==0):
						gestureTest.append(round((np.float64(readList[index][k])-np.float64(readList[index][10]))**2,2))
					else:
						gestureTest.append(round((np.float64(readList[index][k])-np.float64(readList[index][11]))**2,2))


			self.__answerTest.append(np.float64(readList[index][k+1]))
			self.__testSet.append(gestureTest)



	#Tem que corrigir
	def trainingOvR(self):
		print("... training model One - vs- Rest")
		clf = svm.LinearSVC()
		clf.fit(self.__trainSet,self.__answerTrain)
		print ("... testing model")

		#Realiza os testes
		classPredictndArray = clf.predict(self.__testSet)
		self.__verifyTest(classPredictndArray)

	def trainingOvO(self):
		print("... training model One - vs- One")
		clf = svm.SVC(decision_function_shape='ovo',kernel='linear')


		clf.fit(self.__trainSet,self.__answerTrain)

		print ("... testing model")
		#Realiza os testes
		classPredictndArray = clf.predict(self.__testSet)
		self.__verifyTest(classPredictndArray)

	def decisionTreeTraining(self):
		print("... training model with Decision Tree")
		clf = tree.DecisionTreeClassifier()
		print(self.__trainSet)
		clf.fit(self.__trainSet,self.__answerTrain)
		#Realiza os testes
		print ("... testing model")
		classPredictndArray = clf.predict(self.__testSet)
		self.__verifyTest(classPredictndArray)

	def sGradientDescent(self):
		print("... training model with Stochastic Gradient Descent")
		clf = SGDClassifier(loss="modified_huber", penalty="elasticnet")
		clf.fit(self.__trainSet,self.__answerTrain)
		#Realiza os testes
		print ("... testing model")
		classPredictndArray = clf.predict(self.__testSet)
		self.__verifyTest(classPredictndArray)

	def __verifyTest(self,predictArray):
		correct = 0
		incorrect = 0
		for i in xrange(len(self.__testSet)):

			#Verifica porcentagem de acerto
			if(predictArray[i]==self.__answerTest[i]):
				correct+=1
			else:
				incorrect+=1

		self.__statistics(correct,incorrect)

	#v1 indica as corretas
	#v2 indica as incorretas
	def __statistics(self,v1,v2):
		#Total de gestos no conjunto de testes
		totGesture = np.float64(len(self.__testSet))
		print("########################################")
		print("# ")
		print("#     Estatisticas de Treinamento")
		print("# ")
		print("#  Gestos para Treinamento: "+str(len(self.__trainSet)))
		print("#  Gestos para Teste: "+str(len(self.__testSet)))
		print("#  Corretas "+str(v1))
		print("#  Incorretas "+str(v2))
		pc = v1/totGesture
		print("#  % Predicoes corretas: "+str(pc))
		erro = v2/totGesture
		print("#  % Erro: "+str(erro))
		print("########################################")

	#So serve para automatizar o script
	def __statisticsTest(self,v1,v2,tip):
		totGesture = np.float64(len(self.__testSet))
		pc = v1/totGesture
		erro = v2/totGesture
		print(tip+","+str(v1)+","+str(v2)+","+str(pc)+","+str(erro))

	def transformData(self,pathFile):
		readList = []
		name = "..\\Kinect2Libras\\Kinect2Libras\\Dataset\\gestureDataSet.txt"
		gestureFile = []
		lastNum = len(readList)-1
		print ("...load data set "+pathFile)
		with open(pathFile, 'rb') as f:
			reader = csv.reader(f)
			readList = list(reader)

		#Realiza a mistura do data set, para nunca ter o mesmo esquema de gestos
		print ("Total of Gesture: "+str(len(readList)))
		print ("... mixed dataSet")
		random.shuffle(readList)

		#Normaliza todos com o centro de cada mao
		try:
			fileDataset = open(name,'a')   # Trying to create a new file or open one,

			print "wo"
			for i in xrange(int(len(readList))):
				w = 0
				currentGesture = ""
				currentGesture+= ""+str(readList[i][lastNum])+" "
				for k in xrange(len(readList[0])-1):
					if((k!=10)and(k!=11)):
						if(k%2==0):
							currentGesture+=str(w+1)+":"+str(round((np.float64(readList[i][k])-np.float64(readList[i][10]))**2,2))+" "
							w+=1
						else:
							currentGesture+=str(w+1)+":"+str(round((np.float64(readList[i][k])-np.float64(readList[i][11]))**2,2))+" "
							w+=1
				fileDataset.write(currentGesture+"\n")
		except:
			print('Something went wrong! Can\'t tell what?')
			sys.exit(0) # quit Python

		fileDataset.close()

if __name__ == '__main__':
	x = SVMTraining(np.float64(sys.argv[3]))
	x.readingData(sys.argv[1])
	if(sys.argv[2] == "ovo"):
		x.trainingOvO()
	elif(sys.argv[2] == "ovr"):
		x.trainingOvR()
	elif(sys.argv[2] == "dtn"):
		x.decisionTreeTraining()
	elif(sys.argv[2] == "sgd"):
		x.sGradientDescent()
	elif(sys.argv[2] == "sim"):
		x.simulationBestModel()
	elif(sys.argv[2] == "data"):
		x.transformData(sys.argv[1]);
