// ProjectileMotionWithDrag.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <math.h>
#include <float.h>
double A,B,xi0;
double f1(double x);
int iter(double * x, double (*f)(double),double x0,double xmin, double xmax,
	 double eps, int maxiter);
int main()
{

  double v0,vx0,vy0,g,gamma,tmax,dt,x,y;
  int i;
  
  double xt,yt,t;
  
  printf("What is g? Positive points towards ground\n");
  scanf("%lf",&g);
  
  
  printf("what is v0?\n");
  scanf("%lf",&v0);
  while (v0<=0)
    {
      printf("v0 must be greater than or equal to 0\n");
      scanf("%lf",&v0);
    }

  printf("what is gamma?\n");
  scanf("%lf",&gamma);
  while (gamma<0)
    {
      printf("Gamma must be greater than or equal to 0\n");
      scanf("%lf",&gamma);
    }
  
  printf("x must be greater than 0\n");
   scanf("%lf",&x);
  x=0;
  while (x<=0)
    {
      printf("x must be greater than 0\n");
      scanf("%lf",&x);
    }
  
  printf("y =?\n");
  scanf("%lf",&y);
  
  B=gamma*x/v0;
  A=g/(gamma*gamma*x);
  if(B>=1)
    {
      printf("no solutions, enter 0 to quit\n");
	scanf("%d",&i);
      return 0;
    }
  else
    {
      double xi=0;
      double Bs,xib=sqrt(1/(B*B)-1);
      double xi1=0;
      double eps;
      int maxiter,nsol=1;
      xi0=y/x;      
      printf("what is eps=?\n");
      scanf("%lf",&eps);
      printf("what is maxiter=?\n");
      scanf("%d",&maxiter);
      nsol=iter(&xi1,f1,xi,-xib,xib,eps, maxiter);
      if(nsol)
	{
	  FILE * ff;
	  char fname[100];
	  vx0=v0*cos(atan(xi1));
	  vy0=v0*sin(atan(xi1));
	  printf("%lf %lf\n",vx0,vy0);
	  printf("what is tmax\n");
	  scanf("%lf",&tmax);
	  while (tmax<0)
	    {
	      printf("Tmax must be greater than or equal to 0\n");
	      scanf("%lf",&tmax);
	    }
	  
	  printf("what is dt\n");
	  scanf("%lf",&dt);
	  while (dt<=0)
	    {
	      printf("Dt must be greater than 0\n");
	      scanf("%lf",&dt);
	    }

	  printf("What is filename?\n");
	  scanf("%s",fname);
	  // if(!strlen(fname))return 0;
	  // strcat(fname,".csv");
	  ff=fopen(fname,"w");
	  t=0;
	  while(t<tmax)
	    { 
	      xt=vx0*(1-exp(-t*gamma))/gamma;
	      yt=-g*t/gamma+(g/gamma+vy0)*((1-exp(-t*gamma))/gamma);
	      fprintf(ff," %lf %lf %lf\n",t,xt,yt);
	      if(xt>x)break;
	      t+=dt;
	    }
	  fclose(ff);
	}
  else
    printf("no solutions\n");
    }
  char temp[10];
	printf("pause\n");
	scanf("%s",temp);
    
}
//nsol=iter(&xi1,f1,xi,-xib,xib,eps, maxiter);
int iter(double * x, double (*f)(double),double x0,double xmin, double xmax,
	 double eps, int maxiter)
{
  int i=0;
  double x2=x0;
  double x1;
  do
    { 
      x1=x2;
      printf("%lf %lf %lf %d %d\n",x1,xmax,xmin,i,maxiter);
      if((x1>=xmax)||(x1<=xmin))
	{
	return 0;
	}
      if(i>maxiter){
	return 0;
      }
      x2=f(x1);
      i++;
    }
  while(fabs(x2-x1)>=eps);
  *x=x2;
  return 1;
}
double f1(double x)
{
  double Bs=sqrt(1+x*x)*B;
  if(Bs>=1)
    return DBL_MAX;
  else;
  return xi0-A*(log(1-Bs)+Bs);
}



int _tmain(int argc, _TCHAR* argv[])
{
	return 0;
}

