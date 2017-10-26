﻿using HelixToolkit.SharpDX.Core2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using D2D = global::SharpDX.Direct2D1;
using HelixToolkit.Wpf.SharpDX.Extensions;

namespace HelixToolkit.Wpf.SharpDX.Elements2D
{
    public abstract class ShapeModel2D : Model2D
    {
        public static DependencyProperty FillProperty 
            = DependencyProperty.Register("Fill", typeof(Brush), typeof(Model2D), new AffectsRenderPropertyMetadata(new SolidColorBrush(Colors.Black), 
                (d,e)=> 
                {
                    (d as ShapeModel2D).fillChanged = true;
                }));

        public Brush Fill
        {
            set
            {
                SetValue(FillProperty, value);
            }
            get
            {
                return (Brush)GetValue(FillProperty);
            }
        }

        public static DependencyProperty StrokeProperty
        = DependencyProperty.Register("Stroke", typeof(Brush), typeof(Model2D), new AffectsRenderPropertyMetadata(new SolidColorBrush(Colors.Black),
            (d, e) =>
            {
                (d as ShapeModel2D).strokeChanged = true;
            }));

        #region Stroke properties
        public Brush Stroke
        {
            set
            {
                SetValue(StrokeProperty, value);
            }
            get
            {
                return (Brush)GetValue(StrokeProperty);
            }
        }

        public static DependencyProperty StrokeDashCapProperty
        = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(Model2D), new AffectsRenderPropertyMetadata(PenLineCap.Flat,
            (d, e) =>
            {
                (d as ShapeModel2D).strokeStyleChanged = true;
            }));

        public PenLineCap StrokeDashCap
        {
            set
            {
                SetValue(StrokeDashCapProperty, value);
            }
            get
            {
                return (PenLineCap)GetValue(StrokeDashCapProperty);
            }
        }

        public static DependencyProperty StrokeStartLineCapProperty
            = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(Model2D), new AffectsRenderPropertyMetadata(PenLineCap.Flat,
                (d, e) =>
                {
                    (d as ShapeModel2D).strokeStyleChanged = true;
                }));

        public PenLineCap StrokeStartLineCap
        {
            set
            {
                SetValue(StrokeStartLineCapProperty, value);
            }
            get
            {
                return (PenLineCap)GetValue(StrokeStartLineCapProperty);
            }
        }

        public static DependencyProperty StrokeEndLineCapProperty
        = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(Model2D), new AffectsRenderPropertyMetadata(PenLineCap.Flat,
            (d, e) =>
            {
                (d as ShapeModel2D).strokeStyleChanged = true;
            }));

        public PenLineCap StrokeEndLineCap
        {
            set
            {
                SetValue(StrokeEndLineCapProperty, value);
            }
            get
            {
                return (PenLineCap)GetValue(StrokeEndLineCapProperty);
            }
        }

        public static DependencyProperty StrokeDashArrayProperty
            = DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(Model2D), new AffectsRenderPropertyMetadata(new DoubleCollection(),
                (d, e) =>
                {
                    (d as ShapeModel2D).strokeStyleChanged = true;
                }));

        public DoubleCollection StrokeDashArray
        {
            set
            {
                SetValue(StrokeDashArrayProperty, value);
            }
            get
            {
                return (DoubleCollection)GetValue(StrokeDashArrayProperty);
            }
        }

        public static DependencyProperty StrokeDashOffsetProperty
            = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(Model2D), new AffectsRenderPropertyMetadata(0.0,
                (d, e) =>
                {
                    (d as ShapeModel2D).strokeStyleChanged = true;
                }));

        public double StrokeDashOffset
        {
            set
            {
                SetValue(StrokeDashOffsetProperty, value);
            }
            get
            {
                return (double)GetValue(StrokeDashOffsetProperty);
            }
        }

        public static DependencyProperty StrokeLineJoinProperty
        = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(Model2D), new AffectsRenderPropertyMetadata(PenLineJoin.Bevel,
            (d, e) =>
            {
                (d as ShapeModel2D).strokeStyleChanged = true;
            }));


        public PenLineJoin StrokeLineJoin
        {
            set
            {
                SetValue(StrokeLineJoinProperty, value);
            }
            get
            {
                return (PenLineJoin)GetValue(StrokeLineJoinProperty);
            }
        }

        public static DependencyProperty StrokeMiterLimitProperty
            = DependencyProperty.Register("StrokeMiterLimit", typeof(double), typeof(Model2D), new AffectsRenderPropertyMetadata(1.0,
                (d, e) =>
                {
                    (d as ShapeModel2D).strokeStyleChanged = true;
                }));

        public double StrokeMiterLimit
        {
            set
            {
                SetValue(StrokeMiterLimitProperty, value);
            }
            get
            {
                return (double)GetValue(StrokeMiterLimitProperty);
            }
        }

        public static DependencyProperty StrokeThicknessProperty
            = DependencyProperty.Register("StrokeThickness", typeof(int), typeof(Model2D), new AffectsRenderPropertyMetadata(1,
                (d, e) =>
                {
                    if((d as ShapeModel2D).shapeRenderable == null)
                    { return; }   
                    (d as ShapeModel2D).shapeRenderable.StrokeWidth = Math.Max(0, (int)e.NewValue);
                }));

        public int StrokeThickness
        {
            set
            {
                SetValue(StrokeThicknessProperty, value);
            }
            get
            {
                return (int)GetValue(StrokeThicknessProperty);
            }
        }
        #endregion

        private bool fillChanged = true;
        private bool strokeChanged = true;
        private bool strokeStyleChanged = true;

        protected ShapeRenderable2DBase shapeRenderable;

        protected sealed override IRenderable2D CreateRenderCore(IRenderHost host)
        {
            shapeRenderable = CreateShapeRenderCore(host);
            AssignProperties();
            return shapeRenderable;
        }

        protected abstract ShapeRenderable2DBase CreateShapeRenderCore(IRenderHost host);

        protected virtual void AssignProperties()
        {
            shapeRenderable.StrokeWidth = StrokeThickness;
        }

        protected override void OnRenderTargetChanged(D2D.RenderTarget newTarget)
        {
            fillChanged = strokeChanged = strokeStyleChanged = true;
        }

        protected override void PreRender(RenderContext context)
        {
            base.PreRender(context);
            if (fillChanged)
            {
                Disposer.RemoveAndDispose(ref shapeRenderable.FillBrush);
                shapeRenderable.FillBrush = Fill.ToD2DBrush(RenderTarget);
                fillChanged = false;
            }
            if (strokeChanged)
            {
                Disposer.RemoveAndDispose(ref shapeRenderable.StrokeBrush);
                shapeRenderable.StrokeBrush = Stroke.ToD2DBrush(RenderTarget);
                strokeChanged = false;
            }
            if (strokeStyleChanged)
            {
                Disposer.RemoveAndDispose(ref shapeRenderable.StrokeStyle);
                shapeRenderable.StrokeStyle = new D2D.StrokeStyle(RenderTarget.Factory,
                    new D2D.StrokeStyleProperties()
                    {
                        DashCap = this.StrokeDashCap.ToD2DCapStyle(),
                        StartCap = StrokeStartLineCap.ToD2DCapStyle(),
                        EndCap = StrokeEndLineCap.ToD2DCapStyle(),
                        DashOffset = (float)StrokeDashOffset,
                        LineJoin = StrokeLineJoin.ToD2DLineJoin(),
                        MiterLimit = Math.Max(1, (float)StrokeMiterLimit),
                        DashStyle = D2D.DashStyle.Dash
                    }, 
                    StrokeDashArray.Select(x=>(float)x).ToArray());
                strokeStyleChanged = false;
            }
        }
    }
}
