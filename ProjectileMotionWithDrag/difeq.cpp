#include "stdafx.h"
#include <stdio.h>
#include <math.h>
#include <float.h>
#include "pendulum.h"
#include "runge-kutte.h"


int main()
{
  double *y,x,x1,h;
  double pi;
  int n,i,nstep,j;
  FILE * ff;
  char fname[180] = "C:\\Users\\Amichai\\Documents\\Visual Studio 2010\\Projects\\ComputationalPhysics\\ProjectileMotionWithDrag\\Debug\\output.txt";
  y=pendulum_read();
  n=pendulum_get_n();
  x = 0;
  x1 = 30;
  h= .005;
  nstep = 10;
  /*printf("what is initial x?\n");
  scanf("%lf",&x);
  printf("what is final x?\n");
  scanf("%lf",&x1);
  printf("what is step x?\n");
  scanf("%lf",&h);
  printf("number of steps for writing?\n");
  scanf("%d",&nstep);
  printf("what is output_file?\n");
  scanf("%s",fname);*/
  ff=fopen(fname,"w");   
  for(j=0;x<=x1;x+=h,j++)
    {
      if(!(j%nstep))
	{
	  fprintf(ff,"%lf",x);
	  for(i=0;i<n;i++)
	    fprintf(ff," %lf",y[i]);
	  fprintf(ff," %lf %lf %lf\n",pendulum_energy(y),pendulum_kin_energy(),pendulum_pot_energy());
	}
      rk(pendulum_deriv,y,x,h,n);  
    } 
  fclose(ff);
  return 0;
}