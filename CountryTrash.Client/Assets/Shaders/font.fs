#version 330 core

layout(location = 0) out vec4 color;

in vec2 uv;

uniform sampler2D texInterface;

void main()
{
	color.rgb = vec3(0.0f);
	color.a = texture(texInterface, vec2(uv.x, 1.0f - uv.y)).r;
}