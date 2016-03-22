#Script for execution C# with Python

def receive(thumbX,thumbY,indexX,indexY,ringX,ringY,pinkyX,pinkyY, middleX,middleY,centerHandX,centerHandY):
	hand = [thumbX,thumbY,indexX,indexY,ringX, middleX,middleY,ringY,pinkyX,pinkyY,centerHandX,centerHandY]
	normalization(hand) #Serve para normalizar os pontos com a origem (centro da mao)
	#Resultado aqui sera a RESPOSTA QUE ENCONTROU
	print hand

#Normalizara todos de acordo com o centro(centro da mao) 
def normalization(hand):
	for i in range(0,9):
		hand[i] = hand[i] - hand[10]
		hand[i+1] = hand[i+1] - hand[11]
		
def calculateInterphalangea(hand)
	



receive(1,2,3,4,5,6,7,8,9,10,11,12)

