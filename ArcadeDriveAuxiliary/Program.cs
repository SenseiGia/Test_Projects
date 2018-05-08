﻿using System;
using System.Threading;
using Microsoft.SPOT;
using ArcadeDriveAuxiliary.Platform;
using CTRE.Phoenix.MotorControl;

namespace ArcadeDriveAuxiliary
{
    public class Program
    {
        public static void Main()
        {
            /* Disable drivetrain/motors */
            Hardware._rightTalon.Set(ControlMode.PercentOutput, 0);
            Hardware._leftVictor.Set(ControlMode.PercentOutput, 0);

            /* Set Neutral Mode */
            //Need a method to set neutral mode of drivetrain?
            Hardware._rightTalon.SetNeutralMode(NeutralMode.Brake);
            Hardware._leftVictor.SetNeutralMode(NeutralMode.Brake);

            /* Configure output and sensor direction */
            Hardware._rightTalon.SetInverted(true);
            Hardware._leftVictor.SetInverted(false);

            /* Mode print */
            Debug.Print("This is arcade drive using Arbitrary Feedforward");

            while (true)
            {
                /* Enable motor controllers if gamepad connected */
                if (Hardware._gamepad.GetConnectionStatus() == CTRE.Phoenix.UsbDeviceConnection.Connected)
                    CTRE.Phoenix.Watchdog.Feed();

                /* Gamepad value processing */
                float forward = -1 * Hardware._gamepad.GetAxis(1);
                float turn = 1 * Hardware._gamepad.GetAxis(2);
                CTRE.Phoenix.Util.Deadband(ref forward);
                CTRE.Phoenix.Util.Deadband(ref turn);

                /* Use Arbitrary FeedForward to create an Arcade Drive Control by modifying the forward output */
                Hardware._rightTalon.Set(ControlMode.PercentOutput, forward, DemandType.ArbitraryFeedForward, -turn);
                Hardware._leftVictor.Set(ControlMode.PercentOutput, forward, DemandType.ArbitraryFeedForward, +turn);

                Thread.Sleep(5);
            }
        }
    }
}
