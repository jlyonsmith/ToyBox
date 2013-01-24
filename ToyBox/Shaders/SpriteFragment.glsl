varying lowp vec4 DestinationColor;
varying lowp vec2 TexCoordOut;

uniform sampler2D Texture;
uniform vec4 TintColor;
 
void main(void) {
    gl_FragColor = TintColor * texture2D(Texture, TexCoordOut);
}
