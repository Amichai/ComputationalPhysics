#include "stdafx.h"

#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include "pendulum.h"

static int n,i; 
static double g,M1,M2,L,R1,I1,I2,R2,psi,a11,a12,a22;
static double A,B,C,D,E;
static double pe,ke,energy;
static double deg;
double * pendulum_read(void)
{
  FILE * ff;
  char fname[127] = "C:\\Users\\Amichai\\Documents\\Visual Studio 2010\\Projects\\ComputationalPhysics\\ProjectileMotionWithDrag\\Debug\\input.txt";
  double A1,A2;
  double *y;
  n=4;
  y=(double *)malloc(n*sizeof(double));
  ff=fopen(fname,"r");
  fscanf(ff,"%lf",&g);
  fscanf(ff,"%lf",&M1);
  fscanf(ff,"%lf",&I1);
  fscanf(ff,"%lf",&R1);
  fscanf(ff,"%lf",&M2);
  fscanf(ff,"%lf",&I2);
  fscanf(ff,"%lf",&R2);
  fscanf(ff,"%lf",&L);
  fscanf(ff,"%lf",&psi);
  /* energy dissiaton */
  fscanf(ff,"%lf",&a11);
  fscanf(ff,"%lf",&a12);
  fscanf(ff,"%lf",&a22);
  printf("%lf\n",g);
  printf("%lf\n",I1);
  printf("%lf\n",R1);
  printf("%lf\n",M2);
  printf("%lf\n",I2);
  printf("%lf\n",R2); 
  printf("%lf\n",L);
  printf("%lf\n",psi);
  deg=atan((double) 1)/45;
  for(i=0;i<n;i++)
  {
   fscanf(ff,"%lf",y+i);
   y[i]*=deg;
  }
  fclose(ff);
  psi*=deg;
  I1+=M1*R1*R1;
  I2+=M2*R2*R2;
  A1=M1*R1*g;
  A2=M2*L*g;
  A=sqrt(A1*A1+A2*A2+2*A1*A2*cos(psi));
  psi=asin(A1*sin(psi)/A);
  B=M2*R2*g;
  C=I1+L*L*M2;
  D=I2;
  E=M2*R2*L;
  return y;
}
int pendulum_get_n(void)
{return n;}
void pendulum_deriv(double *y, double *dy, double x)
{
  double D1,E1,A1,B1;
  dy[0]=y[2];
  dy[1]=y[3];
  E1=E*sin(y[0]-y[1]);
  A1=-A*sin(y[0]+psi)-E1*y[3]*y[3]-a11*y[2]-a12*y[3];
  B1=-B*sin(y[1])+E1*y[2]*y[2]-a22*y[3]-a12*y[2]; 
  E1=E*cos(y[0]-y[1]);
  D1=1.0/(C*D-E1*E1);
  dy[2]=(A1*D-E1*B1)*D1;
  dy[3]=(B1*C-E1*A1)*D1; 
}
double pendulum_energy(double *y)
{
  pe=-A*cos(y[0]+psi)-B*cos(y[1]);
  ke=(C*y[2]*y[2]+D*y[3]*y[3])*0.5+E*y[2]*y[3]*cos(y[0]-y[1]);
  energy=pe+ke;
  return energy;
}
double pendulum_kin_energy(void)
{
  return ke;
}
double pendulum_pot_energy(void)
{
  return pe;
}