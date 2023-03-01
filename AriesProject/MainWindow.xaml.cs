using Physics;
using Physics.Particles.Generators;
using Physics.Particles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Rendering;

namespace AriesProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        float size = 20;
        float x = 0;
        float y = 0;
        Ellipse particle;
        Rectangle otherRect;
        Rectangle leftEye;
        Rectangle rightEye;
        double eyeY = 20;
        double eyeX = 10;
        double eyeSize = 40;
        DispatcherTimer loopTimer;
        Particle p;
        Particle other;
        GravityGenerator gravity;
        ForceGenerator generator1;
        ForceGenerator generator2;
        FakeSpringGenerator anchor;
        public MainWindow()
        {
            InitializeComponent();
            gravity = new GravityGenerator();
            gravity.Gravity = new System.Numerics.Vector3(0,100,0);
            eyeSize = size / 3;
            eyeX = eyeSize / 4;
            eyeY = size / 6;
            x = (float)Width / 2 - size / 2;
            y = (float)Height / 2 - size / 2;
            particle = new Ellipse(){ Fill = Brushes.Blue, Width = size, Height = size };
            otherRect = new Rectangle { Fill = Brushes.Red, Width = size, Height = size };
            canvas.Children.Add(particle);
            canvas.Children.Add(otherRect);
            p = new Particle();
            other = new Particle();
            p.Position = new System.Numerics.Vector3(300, 400, 0);
            other.Position = new System.Numerics.Vector3(100, 320, 0);
            SetPosition(particle, p.Position.X, p.Position.Y);
            SetPosition(otherRect, other.Position.X, other.Position.Y);
            generator1 = new SpringGenerator(other.Data, 10, 100);
            generator2 = new SpringGenerator(p.Data, 10, 100);
            anchor = new FakeSpringGenerator(new System.Numerics.Vector3(300, 300, 0), 10, 0.99f);
            p.Damping = 0.5f;
            p.Mass =100f;
            other.Mass = 5;
            other.Damping = 0.3f;
            BouyancyGenerator bouyancyGenerator = new BouyancyGenerator(150, 200, 1000);
           // p.AddGenerator(bouyancyGenerator);
            p.AddGenerator(anchor);
           /* p.AddGenerator(generator1);
            other.AddGenerator(generator2);
            other.AddGenerator(gravity);*/
            p.AddGenerator(gravity);

            loopTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            loopTimer.Tick += (object? sender, EventArgs e) =>
            {
                float dt = 16.0f / 1000;
                p.Integrate(dt);
                other.Integrate(dt);
                SetPosition(particle, p.Position.X, p.Position.Y);
                SetPosition(otherRect, other.Position.X, other.Position.Y);
                
            };

            /*
            ImageBrush eyeBrush = new ImageBrush { ImageSource = new BitmapImage(new Uri("Resources/Eyes/puppy.png", UriKind.Relative)),TileMode=TileMode.None };




            leftEye = new Rectangle { Width = eyeSize, Height = eyeSize, Fill = eyeBrush };
            rightEye = new Rectangle { Width = eyeSize, Height = eyeSize, Fill = eyeBrush };
            SetPosition(rightEye, x + size - eyeX, y + eyeY);
            ScaleTransform transform = new ScaleTransform(-1, 1);

            rightEye.RenderTransform = transform;
            SetPosition(leftEye, x + eyeX, y + eyeY);
            
            canvas.Children.Add(leftEye);
            canvas.Children.Add(rightEye);
            */
           // loopTimer.Start();
        }

        void Update()
        {
            if (y + size < ActualHeight)
            {
                y += 15;
            }
            SetPosition(particle, x, y);
            SetPosition(rightEye, x + size - eyeX, y + eyeY);
            SetPosition(leftEye, x + eyeX, y + eyeY);
        }
        void SetPosition(UIElement element, double x, double y)
        {
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, x);
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            using(RenderingWindow window = new RenderingWindow(800,600,"Testing"))
            {
                window.Run();
            }
        }
    }
}
