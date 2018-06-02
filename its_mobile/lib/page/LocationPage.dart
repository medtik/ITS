import 'package:flutter/material.dart';

class LocationPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return new Scaffold(
      appBar: new AppBar(
          backgroundColor: Colors.orange,
          title: new Text("Intelligent Tourist")),
      body: new Column(children: <Widget>[
        new AspectRatio(
          aspectRatio: 3.5 / 2,
          child: new Container(
            decoration: new BoxDecoration(
                image: new DecorationImage(
              fit: BoxFit.fitWidth,
              alignment: FractionalOffset.center,
              image: new AssetImage("assets/banhuot.png"),
            )),
          ),
        ),
//        new DefaultTabController(
//            length: 3,
//            child: new Scaffold(
//              appBar: new AppBar(
//                bottom: new TabBar(
//                  tabs: [
//                    new Tab(icon: new Icon(Icons.directions_car)),
//                    new Tab(icon: new Icon(Icons.directions_transit)),
//                    new Tab(icon: new Icon(Icons.directions_bike)),
//                  ],
//                ),
//              ),
//            ))
      ]),
    );
  }
}
