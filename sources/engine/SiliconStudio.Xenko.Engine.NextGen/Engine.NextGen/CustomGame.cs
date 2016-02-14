using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiliconStudio.Core;
using SiliconStudio.Core.Diagnostics;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Engine.Processors;
using SiliconStudio.Xenko.Games;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Rendering;
using SiliconStudio.Xenko.Rendering.Colors;
using SiliconStudio.Xenko.Rendering.Composers;
using SiliconStudio.Xenko.Rendering.Lights;
using SiliconStudio.Xenko.Rendering.Skyboxes;
using SiliconStudio.Xenko.Rendering.Sprites;

namespace SiliconStudio.Xenko.Engine.NextGen
{
    public class CustomGame : Game
    {
        private TestCamera camera;
        protected Scene Scene;
        protected Entity Camera = new Entity();
        private SceneGraphicsCompositorLayers graphicsCompositor;

        private Model model1, model2;
        private Material material1, material2;
        private NextGenRenderer nextGenRenderer;

        const bool NewSystem = true;

        protected CameraComponent CameraComponent
        {
            get { return Camera.Get<CameraComponent>(); }
            set
            {
                Camera.Add(value);
                graphicsCompositor.Cameras[0] = value;
            }
        }

        public CustomGame()
        {
            GraphicsDeviceManager.DeviceCreationFlags = DeviceCreationFlags.Debug;
        }

        protected override Task LoadContent()
        {
            //Profiler.Enable(GameProfilingKeys.GameDrawFPS);
            ProfilerSystem.EnableProfiling(false, GameProfilingKeys.GameDrawFPS);

            model1 = Asset.Load<Model>("Model");
            model2 = Asset.Load<Model>("Model2");
            material1 = Asset.Load<Material>("Material1");
            material2 = Asset.Load<Material>("Material2");

            SetupScene();

            int cubeWidth = 4;

            var skybox = Asset.Load<Skybox>("Skybox");
            var skyboxEntity = new Entity { new SkyboxComponent { Skybox = skybox }, new LightComponent { Type = new LightSkybox(), Intensity = 1 } };
            Scene.Entities.Add(skyboxEntity);

            for (int i = 0; i < cubeWidth; ++i)
            {
                for (int j = 0; j < cubeWidth; ++j)
                {
                    for (int k = 0; k < cubeWidth; ++k)
                    {
                        var position = new Vector3((i - cubeWidth / 2) * 1.4f, (j - cubeWidth / 2) * 1.4f, (k - cubeWidth / 2) * 1.4f);
                        var material = (k/4)%2 == 0 ? material1 : material2;
                        var isShadowReceiver = (k / 2) % 2 == 0;

                        var entity = new Entity
                        {
                            new ModelComponent { Model = ((i + j + k) % 2) == 0 ? model1 : model2, Materials = { material }, IsShadowReceiver = isShadowReceiver },
                        };
                        entity.Transform.Position = position;
                        Scene.Entities.Add(entity);
                    }
                }
            }

            var spriteSheet = Asset.Load<SpriteSheet>("SpriteSheet");
            var spriteEntity = new Entity
            {
                new SpriteComponent
                {
                    SpriteProvider = new SpriteFromSheet { Sheet = spriteSheet },
                }
            };
            Scene.Entities.Add(spriteEntity);

            camera.Position = new Vector3(35.0f, 5.5f, 22.0f);
            camera.SetTarget(Scene.Entities.Last(), true);

            return base.LoadContent();
        }

        private void SetupScene()
        {
            graphicsCompositor = new SceneGraphicsCompositorLayers
            {
                Cameras = { Camera.Get<CameraComponent>() },
                Master =
                {
                    Renderers =
                    {
                        new ClearRenderFrameRenderer { Color = Color.Green, Name = "Clear frame" },
                        //new SceneCameraRenderer { Mode = new CameraRendererModeForward { Name = "Camera renderer", ModelEffect = "XenkoForwardShadingEffect" } },
                        new SceneCameraRenderer { Mode = nextGenRenderer = new NextGenRenderer { Name = "Camera renderer", ModelEffect = "TestEffect" } },
                    }
                }
            };

            Scene = new Scene { Settings = { GraphicsCompositor = graphicsCompositor } };
            Scene.Entities.Add(Camera);

            //var ambientLight = new Entity { new LightComponent { Type = new LightAmbient { Color = new ColorRgbProvider(Color.White) }, Intensity = 1 } };
            ////var ambientLight = new Entity { new LightComponent { Type = new LightDirectional { Color = new ColorRgbProvider(Color.White) }, Intensity = 1 } };
            ////ambientLight.Transform.RotationEulerXYZ = new Vector3(0.0f, (float) Math.PI, 0.0f);
            //Scene.Entities.Add(ambientLight);

            var directionalLight = new Entity { new LightComponent { Type = new LightDirectional { Color = new ColorRgbProvider(Color.White), Shadow = { Enabled = false } }, Intensity = 1 }, };
            directionalLight.Transform.RotationEulerXYZ = new Vector3((float)Math.PI*-0.3f, (float)Math.PI*-0.3f, (float)Math.PI*-0.3f);
            Scene.Entities.Add(directionalLight);

            //var directionalLight2 = new Entity { new LightComponent { Type = new LightDirectional { Color = new ColorRgbProvider(Color.White), Shadow = { Enabled = true } }, Intensity = 1 }, };
            //directionalLight2.Transform.Rotation = Quaternion.RotationY(MathUtil.PiOverTwo);
            //Scene.Entities.Add(directionalLight2);

            SceneSystem.SceneInstance = new SceneInstance(Services, Scene);

            camera = new TestCamera();
            CameraComponent = camera.Camera;
            Script.Add(camera);
        }
    }
}