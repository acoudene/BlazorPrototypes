window.captureImageFromCamera = async function () {
  const video = document.getElementById('video');
  const canvas = document.getElementById('canvas');
  const context = canvas.getContext('2d');

  if (!video.srcObject) {
    const stream = await navigator.mediaDevices.getUserMedia({ video: true });
    video.srcObject = stream;
  }

  context.drawImage(video, 0, 0, canvas.width, canvas.height);
  return canvas.toDataURL('image/png');
};