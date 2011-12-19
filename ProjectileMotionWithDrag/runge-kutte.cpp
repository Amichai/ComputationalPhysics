#include "stdafx.h"

#include <stdlib.h>

void rk1(void(*deriv)(double*, double*, double),
double * y, double * k1, double *y1, double x, double h,int n)
{ double *k2, *k3, *k4, c3=1.0/3.0, h2=h*0.5, c6=c3*h2;
 int i,j;

 k2=(double *)malloc(n*sizeof(double));
 k3=(double *)malloc(n*sizeof(double));
 k4=(double *)malloc(n*sizeof(double));


 for(i=0;i<n;i++)
   {
     k4[i]=y[i]+k1[i]*h2;
   }
 (*deriv)(k4,k2,x+h*0.5);
 for(i=0;i<n;i++)
   {
     k2[i]*=h;
     k4[i]=y[i]+k2[i]*0.5;
   }
 (*deriv)(k4,k3,x+h*0.5);
 for(i=0;i<n;i++)
   {
     k3[i]*=h;
     k4[i]=y[i]+k3[i];

   }
 (*deriv)(k4,y1,x+h);
 for(i=0;i<n;i++)
   {
     y1[i]=y[i]+(k2[i]+k3[i])*c3+(y1[i]+k1[i])*c6;
    }

 free(k3);
 free(k4);
 return;
}
void rk(void(*deriv)(double*, double*, double),
double * y, double x, double h,int n)
{ double * k1, *k2, *k3, *k4, c3=1.0/3.0, c6=c3*0.5;
 int i,j;
 k1=(double *)malloc(n*sizeof(double));
 k2=(double *)malloc(n*sizeof(double));
 k3=(double *)malloc(n*sizeof(double));
 k4=(double *)malloc(n*sizeof(double));

 (*deriv)(y,k1,x);
 for(i=0;i<n;i++)
   {
     k1[i]*=h;
     k4[i]=y[i]+k1[i]*0.5;
   }
 (*deriv)(k4,k2,x+h*0.5);
 for(i=0;i<n;i++)
   {
     k2[i]*=h;
     k4[i]=y[i]+k2[i]*0.5;
   }
 (*deriv)(k4,k3,x+h*0.5);
 for(i=0;i<n;i++)
   {
     k3[i]*=h;
     k4[i]=y[i]+k3[i];
     y[i]+=k1[i]*c6+(k2[i]+k3[i])*c3;
   }
 (*deriv)(k4,k1,x+h);
 for(i=0;i<n;i++)
   {
     y[i]+=k1[i]*h*c6;
   }
 free(k1);
 free(k2);
 free(k3);
 free(k4);
 return;
}
static double *a;
static int state;

void leapfrog_init(void(*lf)(double*, double *),
		   double * y, double h, int n)
{
  double *v;
  int i,n2=n>>1;
  double h2=h*0.5;
  v=y+n2;
  a=(double *)malloc(n2*sizeof(double));
  (*lf)(y,a);
  for(i=0;i<n2;i++)
    v[i]-=a[i]*h2;
  state=1;
  return;
}


void leapfrog_correct(double * y, double h,int n)
{
  double *v;
  int i,n2=n>>1;
  double h2=state*h*0.5;
  state=-state;  
  v=y+n2;
  for(i=0;i<n2;i++)
    v[i]+=a[i]*h2;
  return;
}


void leapfrog(void(*lf)(double*, double *),
	      double * y, double h, int n )
{ 
  double *v;
  int i,n2=n>>1;
   v=y+n2;
   (*lf)(y,a);
  state=1;
 for(i=0;i<n2;i++)
    v[i]+=a[i]*h; 
 for(i=0;i<n2;i++)
    y[i]+=v[i]*h;
 

  return;
}
void leapfrog_close(void)
{ free(a);return;}