﻿using System;
using Unity.Properties;
using Unity.Properties.UI;
using UnityEditor;
using UnityEngine.UIElements;

namespace Unity.Build.Editor
{
    interface IChangeHandler
    {
        event Action OnChanged;
    }

    sealed class HierarchicalComponentContainerElement<TContainer, TComponent, T> : VisualElement, IChangeHandler, IBinding, IBindable
        where TContainer : HierarchicalComponentContainer<TContainer, TComponent>
        where T : TComponent
    {
        internal static class ClassNames
        {
            public const string BaseClassName = nameof(HierarchicalComponentContainer<TContainer, TComponent>);
            public const string Component = BaseClassName + "__component";
            public const string Header = BaseClassName + "__component-header";
            public const string Inherited = BaseClassName + "__inherited-component";
            public const string Overridden = BaseClassName + "__overridden-component";
            public const string RemoveComponent = BaseClassName + "__remove-component-button";
            public const string Fields = BaseClassName + "__component-fields";
        }

        readonly HierarchicalComponentContainer<TContainer, TComponent> m_Container;
        readonly PropertyElement m_Element;
        readonly Button m_RemoveButton;
        readonly Label m_MissingComponentLabel;

        public event Action OnChanged = delegate { };

        public HierarchicalComponentContainerElement(HierarchicalComponentContainer<TContainer, TComponent> container, T component)
        {
            this.AddStyleSheetAndVariant(ClassNames.BaseClassName);

            m_Container = container;

            AddToClassList(ClassNames.BaseClassName);

            var componentContainerName = component.GetType().Name;
            var foldout = new Foldout { text = ObjectNames.NicifyVariableName(componentContainerName) };
            foldout.AddToClassList(ClassNames.Component);
            foldout.AddToClassList(componentContainerName);
            Add(foldout);
            var toggle = foldout.Q<Toggle>();
            toggle.AddToClassList(ClassNames.Header);

            m_RemoveButton = new Button(RemoveComponent);
            m_RemoveButton.AddToClassList(ClassNames.RemoveComponent);
            toggle.Add(m_RemoveButton);

            m_Element = new PropertyElement();
            m_Element.OnChanged += ElementOnOnChanged;
            m_Element.SetTarget(component);

            foldout.contentContainer.Add(m_Element);
            foldout.contentContainer.AddToClassList(ClassNames.Fields);

            m_MissingComponentLabel = new Label($"Component of type {typeof(T).Name} is missing");
            m_MissingComponentLabel.style.display = DisplayStyle.None;
            foldout.contentContainer.Add(m_MissingComponentLabel);

            SetBorderColor();
        }

        void RemoveComponent()
        {
            m_Container.RemoveComponent<T>();
            if (m_Container.HasComponent<T>())
            {
                m_Element?.SetTarget(m_Container.GetComponent<T>());
                SetBorderColor();
            }
            else
            {
                RemoveFromHierarchy();
            }

            OnChanged();
        }

        void SetBorderColor()
        {
            if (m_Container.IsComponentInherited<T>())
            {
                AddToClassList(ClassNames.Inherited);
                m_RemoveButton.style.display = DisplayStyle.None;
            }
            else
            {
                RemoveFromClassList(ClassNames.Inherited);
            }

            if (m_Container.IsComponentOverridden<T>())
            {
                AddToClassList(ClassNames.Overridden);
                m_RemoveButton.style.display = DisplayStyle.Flex;
            }
            else
            {
                RemoveFromClassList(ClassNames.Overridden);
            }
        }

        void ElementOnOnChanged(PropertyElement element, PropertyPath path)
        {
            m_Container.SetComponent(element.GetTarget<T>());
            element.SetTarget(m_Container.GetComponent<T>());
            SetBorderColor();
            OnChanged();
        }

        public void PreUpdate()
        {
        }

        public void Update()
        {
            if (m_Container.HasComponent<T>())
            {
                m_Element.style.display = DisplayStyle.Flex;
                m_MissingComponentLabel.style.display = DisplayStyle.None;
                m_Element.SetTarget(m_Container.GetComponent<T>());
            }
            else
            {
                m_Element.style.display = DisplayStyle.None;
                m_MissingComponentLabel.style.display = DisplayStyle.Flex;
            }
        }

        public void Release()
        {
        }

        public IBinding binding
        {
            get => this;
            set { }
        }
        public string bindingPath { get; set; }
    }
}
