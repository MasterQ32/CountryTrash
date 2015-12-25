#version 330 core

layout(location = 0) out vec4 color;

in vec2 uv;

uniform sampler2D texInterface;

void main()
{
	color = texture(texInterface, uv);
}