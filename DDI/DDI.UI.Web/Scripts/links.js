
var Links = [];

function LoadLinks(entity) {

    Links = GetLinks(entity);

}

function GetLinks(entity) {

    var links = '';

    if (entity && entity.Links) {

        //$.map(entity.Links, function (link) {

        //    var relationship = link.Relationship;

        //    links += "{" + relationship + ": {" +
        //                'Href: ' + "'" + link.Href + "'" +
        //                ', Method: ' + "'" + link.Method + "'}}";

        //});

        //links = '{' + links + '}'

        links = JSON.parse(entity.Links);

        return links;

    }

}



