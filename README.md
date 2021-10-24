
# PiQuad

**Disclaimer**<br>
*PiQuad is a hobby project with the sole purpose of learning about quadcopters. This repository will contain all information needed to reproduce the steps necessary to create your own quadcopter, controlled by a Raspberry Pi Zero (W). If you're coming here hoping to learn how to create an awesome quadcopter: look elsewhere. This isn't for you. However, if you're coming here hoping to create a basic, functional quadcopter which you can control with a Raspberry Pi and basic Python knowledge, this might just be the place to be.*

## Hardware
![Drone Overview Shot](https://i.ibb.co/v36qZ3k/drone-top-down.png)

Below is a list of the hardware used for this project. As explained in the disclaimer, this is definitely not a list of the best hardware to use. It's just meant to document exactly what was used for the project, in order to reflect on the hardware choices later on. Feel free to order these parts, but keep in mind you may be wasting your money. If you want to design your own quadcopter, and need to pick your motors and propellors, there's a calculator to help you pick your parts [over here][Calculator].

#### Total Cost: &euro; 181.53*
| Part Name | Item | Cost |
| ------ | ------ | ------ |
| Frame | [GEPRC Mark4 FPV Drone Frame Kit 5"][Frame] | &euro; 44.32 |
| Motors | [Amax Performante 2306 V1 1750KV][Motors] | &euro; 55.80 |
| Propellors | [HQ Ethix S5 5x4x3 (5040) 3-blade][Propellors] | &euro; 2.66 |
| Electronic Speed Controller | [Racerstar Air50 3-6S 50A Speed Controller][ESC] | &euro; 36.36 |
| Battery | [Sunpadow LiPo 1400 mAh 22.2V (6S) 130C][Battery] | &euro; 26.79 |
| Accelerometer & Gyroscope | [MPU-6050 3-Axis Gyroscope + Accelerometer][Gyro] | &euro; 1.17 |
| DC-DC Step-Down Converter | [LM2596 DC-DC Adjustable Step-Down Power Supply][LM2596] | &euro; 0.82 |
| DC Voltage & Current Sensor | [INA219 Voltage and Current Sensor Module][INA219] | &euro; 2.11 |
| Flight Controller | [Raspberry Pi Zero W][RPi] | &euro; 11.50 |

Additional components and equipment you'll need:
- Soldering iron + solder (to solder the motor wires to the ESC and the ESC control wires to the prototyping PCB / Raspberry Pi)
- Medium-strength threadlock (for the screws in the frame)
- Additional XT60 connector (to attach the battery to the ESC)
- Electrical tape (optional: frame comes with zip ties)
- Prototyping PCB and female header (optional: you can solder directly to the pi)
- Micro-SD card for the Raspberry Pi

\* The total cost is excluding the additional (and optional) components and equipment
## Software

[Calculator]: <https://www.ecalc.ch/xcoptercalc.php>
[Frame]: <https://www.aliexpress.com/item/4000243274017.html>
[Motors]: <https://amaxshop.com/index.php?route=product%2Fproduct&path=68_69&product_id=488>
[Propellors]: <https://www.aliexpress.com/item/1005001906904891.html>
[ESC]: <https://www.aliexpress.com/item/1005001774084238.html>
[Battery]: <https://www.aliexpress.com/item/1005003135607831.html>
[Gyro]: <https://www.aliexpress.com/item/32761922595.html>
[LM2596]: <https://www.aliexpress.com/item/32952599622.html>
[INA219]: <https://www.aliexpress.com/item/1005001621733824.html>
[RPi]: <https://www.raspberrypi.com/products/raspberry-pi-zero-w/>
