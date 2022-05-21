﻿using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Text;
using static ScrapBox.Framework.ECS.Collider;

namespace JumpMan.Objects
{
    public class TeleportPlatform : Entity
    {
    
       
        
            public override string Name => "Tileable Platform";

            public Transform Transform;
            public Sprite2D Sprite;
            public RigidBody2D Rigidbody;
            public BoxCollider2D Collider;
            public Player player;
        public bool succes;

            public TeleportPlatform(string texture, ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
            {
                Transform = new Transform
                {
                    Position = position,
                    Dimensions = dimensions
                };

                RegisterComponent(Transform);

                Rigidbody = new RigidBody2D
                {
                    IsStatic = true
                };

                RegisterComponent(Rigidbody);

                Collider = new BoxCollider2D
                {
                    Algorithm = CollisionAlgorithm.SAT,
                    Dimensions = dimensions
                };

                RegisterComponent(Collider);

                Sprite = new Sprite2D
                {
                    Texture = AssetManager.FetchTexture(texture),
                    Mode = SpriteMode.TILE
                };

                RegisterComponent(Sprite);
            }

        public void TelportP()
        {
            if (Collision.IntersectPolygons(Collider.GetVerticies(), player.Collider.GetVerticies(), out CollisionManifold manifold))
            {
                
                player.Transform.Position += new ScrapVector(-900, 120);
            }
        }

            public override void Awake()
            {
            succes = Dependency<Player>(out player);
            base.Awake();
            }

            public override void Sleep()
            {
            
            base.Sleep();
            }

            public override void PreLayerTick(double dt)
            {
                base.PreLayerTick(dt);
            }

            public override void PostLayerTick(double dt)
            {
                base.PostLayerTick(dt);
            TelportP();

            }

            public override void PreLayerRender(Camera camera)
            {
                base.PreLayerRender(camera);
            }

            public override void PostLayerRender(Camera camera)
            {
                base.PostLayerRender(camera);
            }
        }
    }
