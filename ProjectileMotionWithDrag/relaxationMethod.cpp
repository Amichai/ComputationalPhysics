#include "stdafx.h"
#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#define mult (314159261)
#define mult1 (314159269)
#define add (907633385)
#define big 4294967296.0

void savebmp(char * fname, short * s,int step){ 
  unsigned char * color;
  int bmp[269];
  FILE *ff;
  int i,lx,ly,ns;
  lx=step;
  ly=step;
  ns=step*step;
  ff=fopen(fname,"wb");
  color=(unsigned char*) malloc(2*sizeof(unsigned char));
  color[0]='B';
  color[1]='M';
  fwrite(&color[0],1,2,ff);
  for(i=0;i<269;i++)
    bmp[i]=0;
  bmp[0]=lx*ly*3+1078;
  bmp[2]=1078;
  bmp[3]=40;
  bmp[4]=lx;
  bmp[5]=ly;
  bmp[6]=524289;
  bmp[8]=lx*ly*3;
  bmp[9]=2834;
  bmp[10]=2834;
  bmp[11]=256;
  bmp[12]=256;
    for(i=0;i<256;i++)
      bmp[13+i]=i*65793;
  color[0]=255;
  color[1]=0;
  fwrite(&bmp[0],4,269,ff);
  for(i=0;i<ns;i++)
    fwrite(&color[(s[i]+1)>>1],1,1,ff);
  fclose(ff);
}


int main()
{
  unsigned int rn,rn1,prn;
  int i,j,k,l,n,nrun,irun,step,step1;
  int mag,iT,nT;
  int en;
  double avmag,aven,amag;
  double H,H0,dH;
  int nH,iH,inde;
  short * s;
  FILE *ff;
  //char fname[80],
  char fnamebmp[80];
  int dz[6];
  double ex[6];
  double T,T0,dT,p;
  double fact,fact1,fn;
  //printf("out name?\n");
  //scanf("%s",fname);
  char fname[80] = "out.txt";
  ff=fopen(fname,"w");
  //printf("what is L?\n");
  //scanf ("%d",&step);
  step = 20;
  /*printf("what is nrun?\n");
  scanf ("%d",&nrun);*/
  nrun = 500;
  /*printf("what is average length?\n");
  scanf ("%d",&n);*/
  n = 5;
  /* n tells how many MC steps is taken between two outputs */
  fn=((double)1)/n;
  /*printf("what is T0?\n");
  scanf ("%lf",&T0);*/
  T0 = 100;
  dT = .05;
  nT = 5;

  H0 = 0;
  /*printf("what is dH\n");
  scanf ("%lf",&dH);*/
  dH = .1;
  /*printf("what is nH?\n");
  scanf ("%d",&nH);*/
  nH = 50;

  /* step1 is the total number of spins */ 
  step1=step*step;
  /*dz is the array of address differences of the nearest neighbors*/
  dz[0]=1;
  dz[1]=step;
  dz[2]=-1;
  dz[3]=-step;
  /* array of spins */
  s=(short *)malloc(step1*sizeof(short));

  /* rundom number generator coefficients */
  fact= ((double)step1)/big;
  fact1= ((double)1)/big;


  for(iT=0;iT<=nT;iT++)
    {
    T=T0+dT*iT;

  for(iH=0;iH<=nH;iH++)
  {
    H=H0+dH*iH;
    /* probabilities to increase the potential energy */
    /* we assume that -2<H<2, thus if energy change due to spin interactions
       is negative, the gross change in energy is also negative */
    ex[0]=exp(-2*H/T);
    ex[2]=exp(-4/T-2*H/T);
    ex[4]=exp(-8/T-2*H/T);
    ex[1]=exp(2*H/T);
    ex[3]=exp(-4/T+2*H/T);
    ex[5]=exp(-8/T+2*H/T);
    /* spin initialization */
  for(i=0;i<step1;i++)
    {
      rn=rn*mult+add;
      if(rn>>31)
	s[i]=1;
      else 
	s[i]=-1;
    }
  /* en -energy, mag- magnetization initialization */
  en=0;
  mag=0;
  for(i=0;i<step1;i++)
    {
      int eni=0; 
      for(k=0;k<4;k++)
	{
	  j=i+dz[k];
	  if(j>=step1)j-=step1;
	  if(j<0)j+=step1;
	  eni+=s[j];
	}
      if(s[i]<0)
	en-=eni;
      else 
	en+=eni;
      mag+=s[i];
    }
  en>>=1;

  for(irun=0;irun<nrun;irun++)
    {
      aven=0;
      avmag=0;
      amag=0;
      for(l=0;l<n;l++)
	{
	  int de=0;
	  rn=rn*mult+add;
	  i=rn*fact;
	  /* select at random i-th spin to flip */ 
	  for(k=0;k<4;k++)
	    {
	      j=i+dz[k];
	      if(j>=step1)j-=step1;
	      if(j<0)j+=step1;
	      de+=s[j];
	    }
	  /* de is the magnetization of neiboring spins */
	  inde=de;
	  if(s[i]<0){de=-de;inde=-inde+1;}
	  /* de is the energy of spin interaction */
	  /*inde is the index of the Boltzmann factor */
	  if(inde<0)
	    {
	      en-=de+de;
              s[i]=-s[i];
	      mag+=s[i]+s[i];
	    }
	  else
	    {
	      rn=rn*mult+add;
	      p=rn*fact1;
	      if(p<ex[inde])
		{
		  en-=de+de;
		  s[i]=-s[i];
		  mag+=s[i]+s[i];
		}
	    }
 	  aven+=en;
          amag+=mag;
	  avmag+=abs(mag);
	}
      fprintf(ff,"%lf,%lf,%lf,%lf,%lf\n",T,(aven-amag*H)*fn,avmag*fn,amag*fn,H);
      fflush(ff);
	printf("%d %lf %lf %d %d %lf %lf\n",irun,aven*fn,avmag*fn,mag,en,T,H);
    }
     // fprintf(ff,"&\n");
    }
  j=0;

  sprintf(fnamebmp,"%s-%04d.bmp",fname,iT);
  savebmp(fnamebmp,s,step);
  /* 
simple spin output
 for(k=0;k<step;k++)
    {
    for(i=0;i<step;i++)
      {
	printf("%1d",(s[j]+1)>>1);
	j++;
      }
	printf("\n");
	} */

  }
  fclose(ff);
}


