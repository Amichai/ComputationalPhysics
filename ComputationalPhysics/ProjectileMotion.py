from numpy import linalg
from numpy import angle
import math
import Vector as vec
import Angle as ang
import matplotlib.pyplot as plt

class Trajectory:
    velocityVector = None
    g = None
    def __init__(self, g, initialVelocity):
       self.g = g
       self.velocityVector = initialVelocity
    def GetLaunchAngle(self):
       return self.velocityVector.angle()
    def GetInitialVelocity(self):
       return self.velocityVector.getMagnitude()
    def PositionAtGivenX(self, xVal):
       time = xVal / self.velocity.xProjection()
       yVal = .5 * g * math.pow(time, 2)
       return [xVal, yVal]
    def PositionAtGivenY(self, yVal):
       time = math.sqrt((2* yVal) / g)
       xVal = velocity.xProjection() * time
       return [xVal, yVal]
    def PositionAtGivenTime(self, time):
       yVal = .5 * self.g * math.pow(time, 2)
       xVal = self.velocityVector.xProjection() * time
       return [xVal, yVal]
    def TimeOfFlight(self):
       return 2 * self.velocityVector.yProjection() / self.g
    def YMax(self):
       time = self.TimeOfFlight() / 2
       return self.PositionAtGivenTime(time)[0]
    def XMax(self):
        return self.PositionAtGivenTime(self.TimeOfFlight())[0]
    def Graph(self, dt = .1):
        xs = []
        ys = []
        t = 0
        while t < self.TimeOfFlight():
            pos = self.PositionAtGivenTime(t)
            xs.append(pos[0])
            ys.append(pos[1])
            t += dt
            
        plt.plot(xs, ys, 'bo')
        plt.axis([min(xs),max(xs),min(ys), max(ys)])
        plt.show()
    
class LaunchTrajectory:
    launchAngles = [None, None]
    def __init__(self, initialVelocity, g = 9.8):
        self.v0 = initialVelocity
        self.g = g
    def __init__(self, initialVelocity, angle, g = 9.8):
        self.v0 = vec.Vector.fromAngleAndMagnitude(angle, initialVelocity)
        self.launchAngles[0] = angle
        self.g = g
    def GetAngleToTarget(self, targetPosition):
        if type(self.v0) is int or type(self.v0) is float:
            A = (self.g * targetPosition[0]) / (self.v0 * self.v0)
            B = (self.g * targetPosition[1]) / (self.v0 * self.v0)
        else:
            A = (self.g * targetPosition[0]) / (self.v0.getMagnitude() * self.v0.getMagnitude())
            B = (self.g * targetPosition[1]) / (self.v0.getMagnitude() * self.v0.getMagnitude())
        D = 1 - 2 * B - A * A;
        if D < 0:
            launchAngles = [None, None]
            return False
        D = math.sqrt(D)
        theta1 = math.atan((1 - D) / A)
        theta2 = math.atan((1 + D) / A)
        self.launchAngles = [ang.Angle(theta1), ang.Angle(theta2)]
        return True
    """Returns trajectory information of the projectile motion over time"""
    def GetTrajectory(self):
        return Trajectory(self.g, vec.Vector.fromAngleAndMagnitude(self.launchAngles[0], self.v0.getMagnitude()))




def main():
    velocityMagnitude = 10
    angle = ang.Angle.fromDegrees(30)
    a = LaunchTrajectory(velocityMagnitude, angle)
    print(a.GetTrajectory().GetLaunchAngle().ToString())
    
    a.GetAngleToTarget([3,3])
    print(a.launchAngles[0].ToString())
    b = a.GetTrajectory()
    print(b.XMax())
    print(b.YMax())
    print(b.TimeOfFlight())
    print(b.GetLaunchAngle().ToString())
    b.Graph(.01)


    b = input("stop here")

if __name__ == "__main__":
    main()