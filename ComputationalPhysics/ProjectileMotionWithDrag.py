import math
import Vector as vec
import Angle as ang

class TrajectoryWithResistance:
    def __init__(self, launchData, pointToHit, gamma):
        if type(launchData.v0) is int or type(launchData.v0) is float:
            self.mag = launchData.v0
        else:
            self.mag = launchData.v0.getMagnitude()
        self.g = launchData.g
        self.x = pointToHit[0]
        self.y = pointToHit[1]
        self.B = gamma * self.x / self.mag
        self.A = self.g / (gamma* gamma* self.x)
        if self.B >= 1:
            print("No Solutions")
            return None
        xi = 0
        xib = math.sqrt(1 / (self.B*self.B) -1)
        self.xi1 =0
        self.xi0 = self.y / self.x
        nsol = self.IterativeSolver(self.f1, xi, xib)
        if nsol == 0:
            print("No Solutions")
            return None
        vx0 = v0 * math.cos(math.atan(self.xi1))
        vy0 = v0 * math.sin(math.atan(self.xi1))
        print(vx0, vy0)
        tmax = 20
        dt = .05
        while t < tmax:
            xt = vx0 * (1 - math.exp( -t * gamma)) / gamma
            yt = - g * t / gammma + (g / gamma + vy0)* ((1-exp(-t*gamma)) / gamma)
            print(t, xt, yt)
            if xt > x:
                break
            t += dt
    def IterativeSolver(self, fToSolve, x0, xib, eps = .001, maxiter = 30):
        i =0
        x2 = x0
        xmin = -xib
        xmax = xib
        while True:
            x1 = x2
            print(x1, xmax, xmin, i, maxiter)
            if x1 >= xmax or x1 <= xmin:
                return 0
            if i > maxiter:
                return 0
            x2 = fToSolve(x1)
            i+=1
            print("Distance from convergence ", math.fabs(x2 - x1))
            if math.fabs(x2 - x1) < eps:
                break
        self.xi1 = x2
        return 1

    def f1(self, x):
       Bs = math.sqrt(1 + x * x)*self.B
       if Bs >=1:
           raise Exception("Problem")
       else:
           return self.xi0 - self.A * (math.log(1 - Bs) + Bs)