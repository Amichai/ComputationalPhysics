import math

class Angle:
    def __init__(self, theta):
       self._180OverPi = 57.2957795131;
       self.theta = theta
    @classmethod
    def fromComponents(cls, xVal, yVal):
       if math.fabs(yVal) == yVal and math.fabs(xVal) == xVal:
           return cls(math.atan(yVal / xVal))
       elif math.fabs(yVal) == yVal and math.fabs(xVal) != xVal:
           return cls(math.atan(yVal / xVal) + math.pi)
       elif math.fabs(yVal) != yVal and math.fabs(xVal) != xVal:
           return cls(math.atan(yVal / xVal) + math.pi)
       elif math.fabs(yVal) != yVal and math.fabs(xVal) == xVal:
           return cls(math.atan(yVal / xVal))

    @classmethod
    def fromDegrees(cls, theta):
        return cls(theta / 57.2957795131)
    
    def Radians(self):
       return self.theta
    def ToString(self):
       return str(self.theta)
