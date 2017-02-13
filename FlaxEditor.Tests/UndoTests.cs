using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = FlaxEngine.Assertions.Assert;


namespace FlaxEditor.Tests
{
    [TestClass]
    public class UndoTests
    {
        [Serializable]
        public class UndoObject
        {
            public int FieldInteger = 10;
            public float FieldFloat = 0.1f;
            public UndoObject FieldObject;

            public int PorpertyInteger { get; set; } = -10;
            public float PorpertyFloat { get; set; } = -0.1f;
            public UndoObject PorpertyObject { get; set; }

            public UndoObject()
            {
            }

            public UndoObject(bool addInstance)
            {
                if (!addInstance)
                {
                    return;
                }
                FieldObject = new UndoObject
                {
                    FieldInteger = 1,
                    FieldFloat = 1.1f,
                    FieldObject = new UndoObject(),
                    PorpertyFloat = 1.1f,
                    PorpertyInteger = 1,
                    PorpertyObject = null
                };
                PorpertyObject = new UndoObject
                {
                    FieldInteger = -1,
                    FieldFloat = -1.1f,
                    FieldObject = null,
                    PorpertyFloat = -1.1f,
                    PorpertyInteger = -1,
                    PorpertyObject = new UndoObject()
                };
            }
        }

        [TestMethod]
        public void UndoBasic()
        {
            var instance = new UndoObject(true);
            Undo.RecordBegin(instance, "Basic");
            instance.FieldFloat = 0;
            instance.FieldInteger = 0;
            instance.FieldObject = null;
            instance.PorpertyFloat = 0;
            instance.PorpertyInteger = 0;
            instance.PorpertyObject = null;
            Undo.RecordEnd();
            Undo.PerformUndo();
            Assert.AreEqual(10, instance.FieldInteger);
            Assert.AreEqual(0.1f, instance.FieldFloat);
            Assert.AreNotEqual(null, instance.FieldObject);
            Assert.AreEqual(-10, instance.PorpertyInteger);
            Assert.AreEqual(-0.1f, instance.PorpertyFloat);
            Assert.AreNotEqual(null, instance.PorpertyObject);
        }
    }
}