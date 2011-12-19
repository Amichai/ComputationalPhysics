extern double * pendulum_read(void);
extern int pendulum_get_n(void);
extern void pendulum_deriv(double *y, double *dy, double x);
extern double pendulum_energy(double *y);
extern double pendulum_kin_energy(void);
extern double pendulum_pot_energy(void);