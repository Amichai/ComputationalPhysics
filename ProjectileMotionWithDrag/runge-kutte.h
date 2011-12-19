extern void rk(void (*deriv)(double*, double*, double),
	       double * y, double x, double h,int n);
extern void rk1(void(*deriv)(double*, double*, double),
	 double * y, double * k1, double *y1, double x, double h,int n);
extern void leapfrog_init(void(*lf)(double*, double *),
			  double * y, double h, int n);
extern void leapfrog_correct(double * y, double h,int n);
extern void leapfrog(void(*lf)(double*, double *),
		     double * y, double h, int n );
extern void leapfrog_close(void);