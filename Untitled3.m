clear;
clc;
P=[-1 -1 2 2 4;0 5 0 5 7];
T=[-1 -1 1 1 -1];
%利用minmax函数求输入样本范围
net = newff(minmax(P),[5,1],{'tansig','purelin'},'trainrp');

net.trainParam.show=50;  %显示训练迭代过程
net.trainParam.lr=0.05;  %学习率
net.trainParam.epochs=300; %最大训练次数
net.trainParam.goal=1e-5; %训练要求精度
[net,tr]=train(net,P,T); %网络训练

W1= net.iw{1, 1}  %输入层到中间层的权值
B1 = net.b{1} %中间各层神经元阈值
W2 = net.lw{2, 1} %中间层到输出层的权值
B2 = net.b{2} %输出层各神经元阈值

sim(net,P) %利用得到的神经网络仿真