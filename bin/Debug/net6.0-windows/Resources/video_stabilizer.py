import matplotlib.pyplot as plt
from vidstab import VidStab

class VideoStabilizer:
    def __init__(self, in_path, out_path):
        self.input_path = in_path
        self.output_path = out_path

    def stabilize(self, kpd_algorithm='FAST', nms=True):
        if (kpd_algorithm=='FAST'):
            self.stabilizer = VidStab(kp_method=kpd_algorithm, nonmaxSuppression=nms)
        else:
            self.stabilizer = VidStab(kp_method=kpd_algorithm)
        self.stabilizer.stabilize(input_path=self.input_path, output_path=self.output_path, border_type='replicate')
