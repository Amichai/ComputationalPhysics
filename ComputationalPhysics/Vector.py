import math
import Angle as ang

class Vector(list):
    @classmethod
    def fromList(cls, args):
        return cls(args)
    @classmethod
    def fromAngleAndMagnitude(cls, angle, magnitude):
        x = magnitude * math.cos(angle.Radians())
        y = magnitude * math.sin(angle.Radians())
        return cls([x,y])
    def numberOfDimensions(self):
        return len(self)

    def angle(self):
        if self.numberOfDimensions() != 2:
            raise Exception("wrong number of dimensions")
        return ang.Angle.fromComponents(self[0], self[1])
    def __add__(self, other):
        return vector(map(lambda x,y: x+y, self, other))
    def __neg__(self):
        return vector(map(lambda x: -x, self))
    
    def __sub__(self, other):
        return vector(map(lambda x,y: x-y, self, other))

    def __mul__(self, other):
        """
        Element by element multiplication
        """
        try:
            return vector(map(lambda x,y: x*y, self,other))
        except:
            # other is a const
            return vector(map(lambda x: x*other, self))
    def dotProduct(self, vec2):
        """
        dot product of two vectors.
        """
        try:
            return reduce(lambda x, y: x+y, a*b, 0.)
        except:
            raise TypeError('vector::FAILURE in dot')
    def getMagnitude(self):
        try:
            return math.sqrt(math.pow(self[0],2) + math.pow(self[1],2))
        except:
            raise TypeError('vector::FAILURE in dot')

    def xProjection(self):
        return math.cos(self.angle().theta) * self.getMagnitude()

    def yProjection(self):
        return math.sin(self.angle().theta) * self.getMagnitude()


