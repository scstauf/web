/* fb_video_extraction.js by scstauf@gmail.com
 *
 * This script demonstrates facebook video extraction.
 *
 * It returns URLs for high definition and standard definition videos.
 */

document.addEventListener('DOMContentLoaded', function() {
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
					.progressive[0];
					
					sdSrc = videoData.sd_src;
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
					.progressive[0];
					
					sdSrc = videoData.sd_src;
					hdSrc = videoData.hd_src;
						
					if (hdSrc) {
						console.log(hdSrc);
					}
					else if (sdSrc) {
						console.log(sdSrc);
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
