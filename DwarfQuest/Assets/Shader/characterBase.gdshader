shader_type canvas_item;

uniform float disintegrate_amount : hint_range(0.0, 1.0) = 0.0;
uniform sampler2D noise_tex : source_color; // e.g. a grayscale noise texture

void vertex() {
	// Called for every vertex the material is visible on.
}

// Called for every pixel the material is visible on.
void fragment() {
    vec4 tex_color = texture(TEXTURE, UV);
    float noise = texture(noise_tex, UV).r;
    if (noise < disintegrate_amount) {
        discard; // pixel disappears
    }
    COLOR = tex_color;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
