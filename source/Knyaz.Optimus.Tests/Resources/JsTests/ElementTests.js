﻿Test("ElementTests", {
    "SetParent": {
        run: function () {
            var div = document.createElement("div");
            var div2 = document.createElement("div");
            Assert.AreEqual(null, div.parentNode);
            div.parentNode = div2;
            Assert.AreEqual(null, div.parentNode);
            div2.appendChild(div);
            Assert.AreEqual(div2, div.parentNode);
        }
    },
    "SetOwnerDocument": {
        run: function () {
            var div = document.createElement("div");
            Assert.AreEqual(document, div.ownerDocument);
            div.ownerDocument = null;
            Assert.AreEqual(document, div.ownerDocument);
        }
    },
    "Remove": {
        run: function () {
            var div = document.createElement("div");
            var childDiv = document.createElement("div");
            div.appendChild(childDiv);
            childDiv.remove();
            Assert.AreEqual(0, div.childNodes.length);
        }
    },
    "AttributesLength": {
        run: function () {
            var div = document.createElement("div");
            div.setAttribute("A", 1);
            div.setAttribute("B", 2);
            Assert.AreEqual(2, div.attributes.length);
        }
    },
    "AttributesGetByName": {
        run: function () {
            var div = document.createElement("div");
            div.setAttribute("A", 1);
            Assert.AreEqual(1, div.attributes["A"].value);
            Assert.AreEqual("a", div.attributes["A"].name);
        }
    },
    "SetOnClickAttribute": {
        run: function () {
            var d = document.createElement('div');
            d.setAttribute('onclick', 'event.currentTarget.innerHTML="HI"');
            d.click();
            Assert.AreEqual("HI", d.innerHTML);
        }
    },
    "SetOnClickAttributePropogation": {
        run: function () {
            var d = document.createElement('div');
            var subDiv = document.createElement('div');
            d.setAttribute('onclick', 'event.currentTarget.innerHTML="HI"');
            d.appendChild(subDiv);
            subDiv.click();
            Assert.AreEqual("HI", d.innerHTML);
        }
    },
    "EventHandlingOrder": {
        run: function () {
            var sequence = [];
            var d1 = document.createElement("div");
            d1.id = "A";
            var d2 = document.createElement("div");
            d2.id = "B";
            d1.appendChild(d2);
            d1.onclick = function (e) { sequence.push('d1 attr - ' + e.eventPhase + e.currentTarget.id); };
            d1.addEventListener("click", function (e) { sequence.push('d1 bubbling - ' + e.eventPhase + e.currentTarget.id) }, false);
            d1.addEventListener("click", function (e) { sequence.push('d1 capture - ' + e.eventPhase + e.currentTarget.id) }, true);
            d2.onclick = function (e) { sequence.push('d2 attr - ' + e.eventPhase + e.currentTarget.id); };
            d2.addEventListener("click", function (e) { sequence.push('d2 bubbling - ' + e.eventPhase + e.currentTarget.id) }, false);
            d2.addEventListener("click", function (e) { sequence.push('d2 capture - ' + e.eventPhase + e.currentTarget.id) }, true);
            d2.click();
            Assert.AreEqual("d1 capture - 1A,d2 attr - 2B,d2 bubbling - 2B,d2 capture - 2B,d1 attr - 3A,d1 bubbling - 3A", sequence.toString());
        }
    },
    "EventListenerParams": {
        run: function () {
            var res = {};
            var elt = document.createElement("div");
            elt.addEventListener = elt.addEventListener("click",
                function (e) {
                    res.e = { target: e.target, currentTarget: e.currentTarget };
                    res.$this = this;
                });
            elt.click();
            Assert.AreEqual(elt, res.$this, "this");
            Assert.AreEqual(elt, res.e.target, "event.target");
            Assert.AreEqual(elt, res.e.currentTarget, "event.currentTarget");
        }
    },
    "EventHandlerParams": {
        run: function () {
            var res = {};
            var elt = document.createElement("div");
            elt.onclick = function (e) {
                res.e = { target: e.target, currentTarget: e.currentTarget };
                res.$this = this;
            };
            elt.click();
            Assert.AreEqual(elt, res.$this, "this");
            Assert.AreEqual(elt, res.e.target, "event.target");
            Assert.AreEqual(elt, res.e.currentTarget, "event.currentTarget");
        }
    },
    "AttrEventHandlerParams": {
        run: function () {
            window.res = {};
            var elt = document.createElement("div");
            elt.setAttribute("onclick",
                "window.res.e = { target: event.target, currentTarget: event.currentTarget };"
                + "window.res.$this = this;");
            elt.click();
            var res = window.res;
            Assert.AreEqual(elt, res.$this, "this");
            Assert.AreEqual(elt, res.e.target, "event.target");
            Assert.AreEqual(elt, res.e.currentTarget, "event.currentTarget");
        }
    }
});