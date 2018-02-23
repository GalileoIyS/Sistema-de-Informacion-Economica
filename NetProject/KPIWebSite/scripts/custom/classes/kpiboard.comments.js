var commentObject = (function ($) {

    //Variables privates
    var _commentid,
        _userid,
        _indicatorid,
        _comment,
        _padreid;

    // Return an object exposed to the public
    return {

        // Peticion de amistad
        insert: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", comment : '" + _comment + "', padreid : " + _padreid + "}";
            proxy.addComment(dataString, funcOK, funcFALSE);
        },

        //cancelamos la amistad
        remove: function (funcOK, funcFALSE) {
            var dataString = "{commentid : " + _commentid + "}";
            proxy.delComment(dataString, funcOK, funcFALSE);
        },

        setCommentId: function (data) {
            _commentid = data;
        },

        setIndicatorId: function (data) {
            _indicatorid = data;
        },

        setComment: function (data) {
            _comment = data;
        },

        setPadreId: function (data) {
            _padreid = data;
        }
    };
} (jQuery));
