/* fb_video_extraction.js by scstauf@gmail.com
 *
 * This script demonstrates facebook video extraction.
 *
 * It returns URLs for high definition and standard definition videos.
 */

// wait for the page to load
document.addEventListener('DOMContentLoaded', function() {
	var embed = document.getElementsByTagName('embed'), // get the embed tags
		flashvars = null, // parse this to get videoData
		videoData = null, // parse this to get sdSrc and hdSrc
		sdSrc = null, // standard definition
		hdSrc = null; // high definition
	
	// if there is an embed tag
	if (embed) {
		// iterate embed tags
		for (var i = 0; i < embed.length; i++) {
			// if flashvars attribute exists
			if (embed[i].hasAttribute('flashvars')) {
				// store it
				var flashvars = embed[i].getAttribute('flashvars');
				// sections are grouped by ampersands, we need the params section
				if (flashvars.length > 0 && flashvars.indexOf('&') > -1) {
					// get the params string, url decode, and parse as an object
					videoData = JSON.parse(
						decodeURIComponent(
							(flashvars.split('&')[0]).split('=')[1]
						)
					)
					.video_data 
					.progressive[0]; // urls are in here
					
					sdSrc = videoData.sd_src; // store the sdSrc and hdSrc
					hdSrc = videoData.hd_src;
						
					// high definition
					if (hdSrc) {
						console.log(hdSrc);
					}
					// standard 
					else if (sdSrc) {
						console.log(sdSrc);
					}
				}
			}
		}
	}
	
	// probably need to return an array of this
	return {
		hd: hdSrc,
		sd: sdSrc
	}
});

/*


(function() {
	var embed = document.getElementsByTagName('embed'),
		flashvars = null,
		videoData = null,
		sdSrc = null,
		hdSrc = null;
	
	if (embed) {
		for (var i = 0; i < embed.length; i++) {
			if (embed[i].hasAttribute('flashvars')) {
				var flashvars = embed[i].getAttribute('flashvars');
				if (flashvars.length > 0 && flashvars.indexOf('&') > -1) {
					videoData = JSON.parse(
						decodeURIComponent(
							(flashvars.split('&')[0]).split('=')[1]
						)
					)
					.video_data
					.progressive;

					if (videoData) {
						sdSrc = videoData.sd_src;
						hdSrc = videoData.hd_src;
					}
				}
			}
		}
	}
	
	return {
		hd: hdSrc,
		sd: sdSrc
	}
})();

*/
