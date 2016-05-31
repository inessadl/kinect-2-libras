# !/usr/bin/pythonw

import os

# This script aims to run the training set for gesture recognition and print the
# stats on a file. It's based on the trainingGestureAlt.py file, on this repository

# To execute:  $ python3 generate_stats.py
# It will write on stats.txt

# TODO keep the same file name and put relative parameters (percent, method)
os.system("echo 'running...'")

file_name = "stats"

os.system("echo '==================================================\n' >> %s.txt" %file_name)
os.system("echo 'Gesture recognition - Brazilian Sign Language\n' >> %s.txt" %file_name)
os.system("echo 'Total number of gestures: 180\n' >> %s.txt" %file_name)

######################### Support Vector Machine - One-vs.One #########################
os.system("echo '==================================================\n' >> %s.txt" %file_name)
os.system("echo 'Support Vector Machine - One-vs.One\n' >> %s.txt" %file_name)
os.system("echo 'Training: 0.7 (125 gestures)' >> %s.txt" %file_name)
os.system("echo 'Evaluation: 0.3 (55 gestures)\n' >> %s.txt" %file_name)
os.system("echo '==================================================\n' >> %s.txt" %file_name)

for i in range(50):
    os.system("python trainingGestureAlt-testing.py fingers.csv ovo 0.7 >> %s.txt" %file_name)
    os.system("echo '-------------------------------------------------' >> %s.txt" %file_name)

os.system("python trainingGestureAlt-testing.py fingers.csv ovo 0.7 >> %s.txt" %file_name)



######################### Support Vector Machine - One-vs.Rest #########################
os.system("echo '\n==================================================\n' >> %s.txt" %file_name)
os.system("echo 'Support Vector Machine - One-vs.Rest\n' >> %s.txt" %file_name)
os.system("echo 'Training: 0.7 (125 gestures)' >> %s.txt" %file_name)
os.system("echo 'Evaluation: 0.3 (55 gestures)\n' >> %s.txt" %file_name)
os.system("echo '==================================================\n' >> %s.txt" %file_name)

for i in range(50):
    os.system("python trainingGestureAlt-testing.py fingers.csv ovr 0.7 >> %s.txt" %file_name)
    os.system("echo '-------------------------------------------------' >> %s.txt" %file_name)

os.system("python trainingGestureAlt-testing.py fingers.csv ovr 0.7 >> %s.txt" %file_name)



######################### Decision Tree #########################
# os.system("echo '\n==================================================\n' >> %s.txt" %file_name)
# os.system("echo 'Decision Tree\n' >> %s.txt" %file_name)
# os.system("echo 'Training: 0.7 (125 gestures)' >> %s.txt" %file_name)
# os.system("echo 'Evaluation: 0.3 (55 gestures)\n' >> %s.txt" %file_name)
# os.system("echo '==================================================\n' >> %s.txt" %file_name)
#
# for i in range(3):
#     os.system("python trainingGestureAlt-testing.py fingers.csv dtn 0.7 >> %s.txt" %file_name)
#     os.system("echo '-------------------------------------------------' >> %s.txt" %file_name)
#
# os.system("python trainingGestureAlt-testing.py fingers.csv dtn 0.7 >> %s.txt" %file_name)
#
# os.system("echo '\n==================================================\n' >> %s.txt" %file_name)



######################### Stochastic Gradient Descent #########################
os.system("echo '\n==================================================\n' >> %s.txt" %file_name)
os.system("echo 'Stochastic Gradient Descent\n' >> %s.txt" %file_name)
os.system("echo 'Training: 0.7 (125 gestures)' >> %s.txt" %file_name)
os.system("echo 'Evaluation: 0.3 (55 gestures)\n' >> %s.txt" %file_name)
os.system("echo '==================================================\n' >> %s.txt" %file_name)

for i in range(50):
    os.system("python trainingGestureAlt-testing.py fingers.csv sgd 0.7 >> %s.txt" %file_name)
    os.system("echo '-------------------------------------------------' >> %s.txt" %file_name)

os.system("python trainingGestureAlt-testing.py fingers.csv sgd 0.7 >> %s.txt" %file_name)

os.system("echo '\n==================================================\n' >> %s.txt" %file_name)


# os.system("echo '' >> %s.txt" %file_name)
# os.system("echo '' >> %s.txt" %file_name)
