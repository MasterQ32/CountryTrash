#version 330 core

layout(location = 0) out vec4 color;

in vec2 uv;
in vec3 normal;

uniform sampler2D texDiffuse;

void main()
{
	vec3 sun = normalize(vec3(0.3f, -0.8f, -0.2f));

	float ambient = 0.3f;
	
	// Special case: Models with "no" normals are bright
	float diffuse = 0.7f * clamp(dot(normal, -sun), 0.0f, 1.0f);
	if(length(normal) < 0.1f)
		diffuse = 0.7f;

	color = vec4(vec3(ambient + diffuse), 1.0f) * texture(texDiffuse, uv);

	if(color.a < 0.5f)
		discard;
}