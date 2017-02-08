﻿
var Links = {};

function LoadLinks(entity) {

    Links = GetLinks(entity);

}

function GetLinks(entity) {

    var links = {};

    if (entity && entity.Links) {

        $.map(entity.Links, function (link) {

            $(Links).add(link.Relationship, {
                Href: link.Href,
                Method: link.Method
            });

        });

        return links;

    }

}



