# Script para execucao automatica de testes de treinamento de gestos capturados pelo Kinect V2
# OS algoritmos utilizados foram SVM, DecisionTree, Stochastic Gradient Descent
# Cada algoritmo sera executado 100x variando a porcentagem de gestos para treinamento em 0.003

import os;

variation = 0.003
percentTrain = 0.65
for k in xrange(0,10):
    for i in xrange(0,50):
        os.system("sudo python /home/lmtortelli/Dropbox/Libras/trainingGestureAlt.py fingers.csv ovo "+str(percentTrain))
    variation+=0.003

variation = 0.003
percentTrain = 0.65
for k in xrange(0,10):
    for i in xrange(0,50):
      os.system("sudo python /home/lmtortelli/Dropbox/Libras/trainingGestureAlt.py fingers.csv ovr "+str(percentTrain))
    percentTrain+=variation
    variation+=0.003

variation = 0.003
percentTrain = 0.65
for k in xrange(0,10):
    for i in xrange(0,50):
      os.system("sudo python /home/lmtortelli/Dropbox/Libras/trainingGestureAlt.py fingers.csv dtn "+str(percentTrain))
    percentTrain+=variation
    variation+=0.003

variation = 0.003
percentTrain = 0.65
for k in xrange(0,10):
    for i in xrange(0,50):
        os.system("sudo python /home/lmtortelli/Dropbox/Libras/trainingGestureAlt.py fingers.csv sgd "+str(percentTrain))
    percentTrain+=variation
    variation+=0.003
