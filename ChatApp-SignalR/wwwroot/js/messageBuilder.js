var messageBuilder = function () {
    var message = null;
    var header = null;
    var p = null;
    var footer = null;

    return {
        createMessage: function (classList) {
            message = document.createElement("div") 

            if (classList === undefined)
                classList = [];

            for (var i = 0; i < classList.length; i++) {
                message.classList.add(classList[i])
            }

            message.classList.add('message')
            message.classList.add('p1')

            return this;
        },

        withHeader: function (text) {
            header = document.createElement("header")
            header.appendChild(document.createTextNode(text + ':'))
            return this;
        },
        withHeader1: function (text) {
            header = document.createElement("header1")
            header.appendChild(document.createTextNode(text + ':'))
            return this;
        },
        withParagraph: function (text) {
            p = document.createElement("p")
            p.appendChild(document.createTextNode(text))
            return this;
        },
        withParagraph1: function (text) {
            p = document.createElement("p1")
            p.appendChild(document.createTextNode(text))
            return this;

        },
        withFooter: function (text) {
            footer = document.createElement("footer")
            footer.appendChild(document.createTextNode(text))
            return this;
        },
        withFooter1: function (text) {
            footer = document.createElement("footer1")
            footer.appendChild(document.createTextNode(text))
            return this;
        },
        build: function () {
            message.appendChild(header);
            message.appendChild(p);
            message.appendChild(footer);
            return message;
        }
    }
}
window.scrollTo(0, document.querySelector(".chat-panel").scrollHeight);