# Script para execucao automatica de testes de treinamento de gestos capturados pelo Kinect V2
# OS algoritmos utilizados foram SVM, DecisionTree, Stochastic Gradient Descent
# Cada algoritmo sera executado 100x variando a porcentagem de gestos para treinamento em 0.003

import os;

variation = 0.003
for i in xrange(0,100):
  os.system("sudo python /home/lmtortelli/Dropbox/Libras/trainingGestureAlt.py fingers.csv ovo "+str(0.65+variation))
  variation+=0.003

variation = 0.003
for i in xrange(0,100):
  os.system("sudo python /home/lmtortelli/Dropbox/Libras/trainingGestureAlt.py fingers.csv ovr "+str(0.65+variation))
  variation+=0.003

variation = 0.003
for i in xrange(0,100):
  os.system("sudo python /home/lmtortelli/Dropbox/Libras/trainingGestureAlt.py fingers.csv dtn "+str(0.70+variation))
  variation+=0.003

variation = 0.003
for i in xrange(0,100):
  os.system("sudo python /home/lmtortelli/Dropbox/Libras/trainingGestureAlt.py fingers.csv sgd "+str(0.70+variation))
  variation+=0.003
