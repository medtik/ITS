import React from "react" ;
import {View, WebView, StyleSheet, Dimensions,BackHandler} from "react-native" ;

const ref = {
  webview: "WEBVIEW_REF"
};
export default class ITSWeb extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            // uri: 'http://its8.gear.host/'
            uri: 'http://192.168.42.14:8080/',
            canGoBack: false
        };
        this.onLayout = this.onLayout.bind(this);
        this.onHardwareBack = this.onHardwareBack.bind(this);
        this.onNavigationStateChange = this.onNavigationStateChange.bind(this);
    }

    componentDidMount() {
        this.onLayout();

        BackHandler.addEventListener('hardwareBackPress', this.onHardwareBack);
    }

    componentWillUnmount() {
        BackHandler.removeEventListener('hardwareBackPress', this.onHardwareBack);
    }

    onLayout() {
        const dimension = Dimensions.get('window');

        this.setState({
            width: dimension.width,
            height: dimension.height
        });
    }

    onHardwareBack(){

        if(this.state.canGoBack){
            this.refs[ref.webview].goBack();
        }
        return this.state.canGoBack;
    }

    onNavigationStateChange(ex){
        this.setState({
            canGoBack: ex.canGoBack
        });
    }

    render() {
        const style = StyleSheet.create({
            container: {
                backgroundColor: "#007AFF",
                paddingTop: 23
            },
            webView: {
                width: this.state.width,
                height: this.state.height,
            }
        });

        console.log(this.state);

        return (
            <View style={style.container}>
                <WebView
                    ref={ref.webview}
                    onNavigationStateChange={this.onNavigationStateChange}
                    onLayout={this.onLayout}
                    source={{uri: this.state.uri}}
                    style={style.webView}
                />
            </View>
        );
    }
}