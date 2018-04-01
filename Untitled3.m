clear;
clc;
P=[-1 -1 2 2 4;0 5 0 5 7];
T=[-1 -1 1 1 -1];
%����minmax����������������Χ
net = newff(minmax(P),[5,1],{'tansig','purelin'},'trainrp');

net.trainParam.show=50;  %��ʾѵ����������
net.trainParam.lr=0.05;  %ѧϰ��
net.trainParam.epochs=300; %���ѵ������
net.trainParam.goal=1e-5; %ѵ��Ҫ�󾫶�
[net,tr]=train(net,P,T); %����ѵ��

W1= net.iw{1, 1}  %����㵽�м���Ȩֵ
B1 = net.b{1} %�м������Ԫ��ֵ
W2 = net.lw{2, 1} %�м�㵽������Ȩֵ
B2 = net.b{2} %��������Ԫ��ֵ

sim(net,P) %���õõ������������