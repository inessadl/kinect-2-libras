# kinect-2-libras

Esta aplicação possui o objetivo de mapear gestos da Língua Brasileira de Sinais (LIBRAS), capturados através do Microsoft Kinect (V2) e salvar os dados em formato compatível com softwares de aprendizado de máquina.

##Introdução
A API Kinect-Finger-Tracking criada por Vitruvius fornece pontos que representam as pontas de cada um dos 5 dedos de cada mão, toda mapeadas diretamente pelo Kinect. Seu princípio de cálculo é aproximar os pontos extremos através de um convexhull para mais informações acesse (github.com/vitruvius/kinect-finger-tracking).

Para o reconhecimento de gestos estáticos em LIBRAS (Língua Brasileira dos Sinais) é necessário capturar a posição da mão alvo e reconhecer padrões para conseguir inferir o gesto observado. Porém únicamente com os pontos fornecidos pela API torna-se impraticavel tal atividade. Uma das possiveis solução é "simular" os efetios do Leap Motion sobre o Kinect, e assim capturar mais pontos na mão.

#Pontos de Interesse
Inicialmente utilizamos de uma informação fornecida pelo próprio Kinect, o centro da mão:
![Centro da mão](/images/centerPoint.png?raw=true "Ponto Central da Mão")

Para obter um dos pontos fornecidos pelo Leap Motion que consiste no encontro entre as falanges médias e as falanges proximais, é necessário utilizar-se do ponto central da mão. Obtendo esta informação, é possível calcular o Ponto Médio entre a ponta de cada dedo com o centro da mão: 

![Localização das falanges da mão](/images/exam.png?raw=true "Localização das falanges da mão")

![Área de cálculo das falanges](/images/calculate.png?raw=true "Área de cálculo das falanges")

Uma vez com a área de cálculo definida, a extração desses pontos da através da angulação no qual eles se encontram. Esta serve para corrigir perturbações que ocorrem pela diferença de Posição do Dedo maior em relação ao demais e do angulo em que cada um econtra-se em relação ao centro. No fim tem-se os seguintes pontos calculados:

![Ponto das Falanges](/images/falanges.png?raw=true "Pontos das falanges")

Com a obtenção dos pontos, cada um segue a seguinte ordem de extração:

![Ponto das Falanges](/images/recordedOrder.png?raw=true "Pontos das falanges")

# Classificação
Para a realização da classificação dos gestos através dos pontos calculados acima, foram utilizados em um primeiro momento 4 implementações diferentes são elas
- SVM One - vs - One
- SVM One - vs - Rest
- Decision Tree
- Stochastical Gradient Descent
 

#Proximas Implementações
- Vetor direção
- Utilização de profundidade para estimar os pontos da mão sob condições adversas
- Ângulo de abertura da mão

## Contribuidores
* Inessa Luerce (idluerce@inf.ufpel.edu.br)
* Lucas Tortelli (lmtortelli@inf.ufpel.edu.br)
* Marilton Sachotene (marilton@inf.ufpel.edu.br)
* Simone Rutz (sdrutz@inf.ufpel.edu.br)
## Referências
Desenvolvido com [Vitruvius](https://github.com/LightBuzz/Vitruvius)
Biblioteca de Classificação [SCIKIT](http://scikit-learn.org/)
