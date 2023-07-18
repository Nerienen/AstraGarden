using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectUtils.Helpers
{
    public static class Transitions
    {
        public enum TimeScales
        {
            Scaled, Unscaled, Fixed
        }

        private static float GetDeltaTime(TimeScales timeScale)
        {
            return timeScale switch
            {
                TimeScales.Scaled => Time.deltaTime,
                TimeScales.Unscaled => Time.unscaledDeltaTime,
                _ => Time.fixedDeltaTime
            };
        }
        
        private static float GetTime(TimeScales timeScale)
        {
            return timeScale switch
            {
                TimeScales.Scaled => Time.time,
                TimeScales.Unscaled => Time.unscaledTime,
                _ => Time.fixedTime
            };
        }

        /// <summary>
        /// <para>Scales the object to a target scale in a determined time</para>
        /// <param name="targetScale">The target scale</param>
        /// <param name="time">The duration in seconds of the scaling effect</param>
        /// </summary>
        public static void DoScale(this Transform transform, Vector3 targetScale, float time, TimeScales timeScale = TimeScales.Unscaled)  =>  CoroutineController.Start(DoScaleEnumerator(transform, targetScale, time, timeScale));
        private static IEnumerator DoScaleEnumerator(Transform transform, Vector3 targetScale, float time, TimeScales timeScale)
        {
            float timer = GetDeltaTime(timeScale);
            Vector3 initialScale = transform.localScale;
            Vector3 scaleDelta = targetScale - initialScale;

            while (timer < time)
            {
                transform.localScale = initialScale + scaleDelta * (timer/time);
                yield return null;
                timer += GetDeltaTime(timeScale);
            }

            transform.localScale = targetScale;
        }

        /// <summary>
        /// <para>Scales the object to a target scale in a determined time</para>
        /// <param name="targetScale">The target scale</param>
        /// <param name="time">The duration in seconds of the scaling effect</param>
        /// </summary>
        public static async Task DoScaleAsync(this Transform transform, Vector3 targetScale, float time, TimeScales timeScale = TimeScales.Unscaled)
        {
            float timer = GetDeltaTime(timeScale);
            Vector3 initialScale = transform.localScale;
            Vector3 scaleDelta = targetScale - initialScale;

            while (timer < time)
            {
                transform.localScale = initialScale + scaleDelta * (timer/time);
                await Task.Yield();
                timer += GetDeltaTime(timeScale);
            }

            transform.localScale = targetScale;
        } 
        
        /// <summary>
        /// <para>Moves the object to a target position in world space in a determined time</para>
        /// <param name="targetPosition">The final position</param>
        /// <param name="time">The duration in seconds of the moving effect</param>
        /// </summary>
        public static void DoMove(this Transform transform, Vector3 targetPosition, float time, TimeScales timeScale = TimeScales.Unscaled)  =>  
            CoroutineController.Start(DoMoveEnumerator(transform, targetPosition, time, timeScale));
        private static IEnumerator DoMoveEnumerator(Transform transform, Vector3 targetPosition, float time, TimeScales timeScale)
        {
            float timer = GetDeltaTime(timeScale);
            Vector3 initialPosition = transform.position;
            Vector3 moveDelta = targetPosition - initialPosition;

            while (timer < time)
            {
                transform.position = initialPosition + moveDelta * (timer/time);
                yield return null;
                timer += GetDeltaTime(timeScale);
            }

            transform.position = targetPosition;
        }

        /// <summary>
        /// <para>Moves the object to a target position in world space in a determined time</para>
        /// <param name="targetPosition">The final position</param>
        /// <param name="time">The duration in seconds of the moving effect</param>
        /// </summary>
        public static async Task DoMoveAsync(this Transform transform, Vector3 targetPosition, float time, TimeScales timeScale = TimeScales.Unscaled)
        {
            float timer = GetDeltaTime(timeScale);
            Vector3 initialPosition = transform.position;
            Vector3 moveDelta = targetPosition - initialPosition;

            while (timer < time)
            {
                transform.position = initialPosition + moveDelta * (timer/time);
                await Task.Yield();
                timer += GetDeltaTime(timeScale);
            }

            transform.position = targetPosition;
        }
        
        /// <summary>
        /// <para>Moves the object to a target position in local space in a determined time</para>
        /// <param name="targetPosition">The final position</param>
        /// <param name="time">The duration in seconds of the moving effect</param>
        /// </summary>
        public static void DoMoveLocal(this Transform transform, Vector3 targetPosition, float time, TimeScales timeScale = TimeScales.Unscaled)  =>  CoroutineController.Start(DoMoveLocalEnumerator(transform, targetPosition, time, timeScale));
        private static IEnumerator DoMoveLocalEnumerator(Transform transform, Vector3 targetPosition, float time, TimeScales timeScale)
        {
            float timer = GetDeltaTime(timeScale);
            Vector3 initialPosition = transform.localPosition;
            Vector3 moveDelta = targetPosition - initialPosition;

            while (timer < time)
            {
                transform.localPosition = initialPosition + moveDelta * (timer/time);
                yield return null;
                timer += GetDeltaTime(timeScale);
            }

            transform.localPosition = targetPosition;
        }

        /// <summary>
        /// <para>Moves the object to a target position in local space in a determined time</para>
        /// <param name="targetPosition">The final position</param>
        /// <param name="time">The duration in seconds of the moving effect</param>
        /// </summary>
        public static async Task DoMoveLocalAsync(this Transform transform, Vector3 targetPosition, float time, TimeScales timeScale = TimeScales.Unscaled)
        {
            float timer = GetDeltaTime(timeScale);
            Vector3 initialPosition = transform.localPosition;
            Vector3 moveDelta = targetPosition - initialPosition;

            while (timer < time)
            {
                transform.localPosition = initialPosition + moveDelta * (timer/time);
                await Task.Yield();
                timer += GetDeltaTime(timeScale);
            }

            transform.localPosition = targetPosition;
        } 
        
        /// <summary>
        /// <para>Moves the object, making a shake movement with a certain magnitude in a determined time</para>
        /// <param name="magnitude">The magnitude of the movement</param>
        /// <param name="time">The duration in seconds of the shaking effect</param>
        /// <param name="moveZ">Determines if the object moves in the z axis</param>
        /// </summary>
        public static void DoShake(this Transform transform, float magnitude, float time, bool moveZ = false, TimeScales timeScale = TimeScales.Unscaled) => 
            CoroutineController.Start(DoShakeEnumerator(transform, magnitude, time, moveZ, timeScale));
        private static IEnumerator DoShakeEnumerator(Transform transform, float magnitude, float time, bool moveZ, TimeScales timeScale)
        {
            float duration = GetTime(timeScale) + time;
            Vector3 initialPosition = transform.position;
            Vector3 newPosition = initialPosition;

            while (GetTime(timeScale) < duration)
            {
                newPosition.x = initialPosition.x + Random.value * magnitude;
                newPosition.y = initialPosition.y + Random.value * magnitude;
                if(moveZ) newPosition.z = initialPosition.z + Random.value * magnitude;
                transform.position = newPosition;
                yield return null;
            }
            transform.position = initialPosition;
        }
        
        /// <summary>
        /// <para>Moves the object, making a shake movement with a certain magnitude in a determined time</para>
        /// <param name="magnitude">The magnitude of the movement</param>
        /// <param name="time">The duration in seconds of the shaking effect</param>
        /// <param name="moveZ">Determines if the object moves in the z axis</param>
        /// </summary>
        public static async Task DoShakeAsync(this Transform transform, float magnitude, float time, bool moveZ = false, TimeScales timeScale = TimeScales.Unscaled)
        {
            float duration = GetTime(timeScale) + time;
            Vector3 initialPosition = transform.position;
            Vector3 newPosition = initialPosition;

            while (GetTime(timeScale) < duration)
            {
                newPosition.x = initialPosition.x + Random.value * magnitude;
                newPosition.y = initialPosition.y + Random.value * magnitude;
                if(moveZ) newPosition.z = initialPosition.z + Random.value * magnitude;
                transform.position = newPosition;
                await Task.Yield();
            }
            transform.position = initialPosition;
        }

        /// <summary>
        /// <para>Rotates the object to a target rotation in world space in a determined time</para>
        /// <param name="targetRotation">The final rotation</param>
        /// <param name="time">The duration in seconds of the rotation effect</param>
        /// </summary>
        public static void RotateTo(this Transform transform, Quaternion targetRotation, float time, TimeScales timeScale = TimeScales.Unscaled) => 
            CoroutineController.Start(RotateToEnumerator(transform, targetRotation, time, timeScale));
        private static IEnumerator RotateToEnumerator(Transform transform, Quaternion targetRotation, float time, TimeScales timeScale)
        {
            float timer = GetDeltaTime(timeScale);
            Quaternion rotation = transform.rotation;

            while (timer < time)
            {
                transform.rotation = Quaternion.Slerp(rotation, targetRotation, timer/time);
                yield return null;
                timer += GetDeltaTime(timeScale);
            }

            transform.rotation = targetRotation;
        }
        
        /// <summary>
        /// <para>Rotates the object to a target rotation in world space in a determined time</para>
        /// <param name="targetRotation">The final rotation</param>
        /// <param name="time">The duration in seconds of the rotation effect</param>
        /// </summary>
        public static async Task RotateToAsync(this Transform transform, Quaternion targetRotation, float time, TimeScales timeScale = TimeScales.Unscaled)
        {
            float timer = GetDeltaTime(timeScale);
            Quaternion rotation = transform.rotation;

            while (timer < time)
            {
                transform.rotation = Quaternion.Slerp(rotation, targetRotation, timer/time);
                await Task.Yield();
                timer += GetDeltaTime(timeScale);
            }

            transform.rotation = targetRotation;
        }
        
        /// <summary>
        /// <para>Rotates the object to a target rotation in local space in a determined time</para>
        /// <param name="targetRotation">The final rotation</param>
        /// <param name="time">The duration in seconds of the rotation effect</param>
        /// </summary>
        public static void RotateToLocal(this Transform transform, Quaternion targetRotation, float time, TimeScales timeScale = TimeScales.Unscaled) => 
            CoroutineController.Start(RotateToLocalEnumerator(transform, targetRotation, time, timeScale));
        private static IEnumerator RotateToLocalEnumerator(Transform transform, Quaternion targetRotation, float time, TimeScales timeScale)
        {
            float timer = GetDeltaTime(timeScale);
            Quaternion rotation = transform.localRotation;

            while (timer < time)
            {
                transform.localRotation = Quaternion.Slerp(rotation, targetRotation, timer/time);
                yield return null;
                timer += GetDeltaTime(timeScale);
            }

            transform.localRotation = targetRotation;
        }
        
        /// <summary>
        /// <para>Rotates the object to a target rotation in local space in a determined time</para>
        /// <param name="targetRotation">The final rotation</param>
        /// <param name="time">The duration in seconds of the rotation effect</param>
        /// </summary>
        public static async Task RotateToLocalAsync(this Transform transform, Quaternion targetRotation, float time, TimeScales timeScale = TimeScales.Unscaled)
        {
            float timer = GetDeltaTime(timeScale);
            Quaternion rotation = transform.localRotation;

            while (timer < time)
            {
                transform.localRotation = Quaternion.Slerp(rotation, targetRotation, timer/time);
                await Task.Yield();
                timer += GetDeltaTime(timeScale);
            }

            transform.localRotation = targetRotation;
        }

        /// <summary>
        /// <para>Rotates the object a certain rotation in world space in a determined time</para>
        /// <param name="rotation">The rotation applied in euler angles</param>
        /// <param name="time">The duration in seconds of the rotation effect</param>
        /// </summary>
        public static void DoRotate(this Transform transform, Vector3 rotation, float time, TimeScales timeScale = TimeScales.Unscaled)
        {
            Quaternion targetRotation = transform.rotation;
            Quaternion quaternion = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            targetRotation *= Quaternion.Inverse(targetRotation) * quaternion * targetRotation;
            
            transform.RotateTo(targetRotation, time, timeScale);
        }

        /// <summary>
        /// <para>Rotates the object a certain rotation in world space in a determined time</para>
        /// <param name="rotation">The rotation applied in euler angles</param>
        /// <param name="time">The duration in seconds of the rotation effect</param>
        /// </summary>
        public static async Task DoRotateAsync(this Transform transform, Vector3 rotation, float time, TimeScales timeScale = TimeScales.Unscaled)
        {
            Quaternion targetRotation = transform.rotation;
            Quaternion quaternion = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            targetRotation *= Quaternion.Inverse(targetRotation) * quaternion * targetRotation;
            
            await transform.RotateToAsync(targetRotation, time, timeScale);
        }
        
        /// <summary>
        /// <para>Rotates the object a certain rotation in local space in a determined time</para>
        /// <param name="rotation">The rotation applied in euler angles</param>
        /// <param name="time">The duration in seconds of the rotation effect</param>
        /// </summary>
        public static void DoRotateLocal(this Transform transform, Vector3 rotation, float time, TimeScales timeScale = TimeScales.Unscaled)
        {
            Quaternion targetRotation = transform.rotation;
            Quaternion quaternion = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            targetRotation *= Quaternion.Inverse(targetRotation) * quaternion * targetRotation;
            
            transform.RotateToLocal(targetRotation, time, timeScale);
        }

        /// <summary>
        /// <para>Rotates the object a certain rotation in local space in a determined time</para>
        /// <param name="rotation">The rotation applied in euler angles</param>
        /// <param name="time">The duration in seconds of the rotation effect</param>
        /// </summary>
        public static async Task DoRotateLocalAsync(this Transform transform, Vector3 rotation, float time, TimeScales timeScale = TimeScales.Unscaled)
        {
            Quaternion targetRotation = transform.rotation;
            Quaternion quaternion = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            targetRotation *= Quaternion.Inverse(targetRotation) * quaternion * targetRotation;
            
            await transform.RotateToLocalAsync(targetRotation, time, timeScale);
        }

        private static Dictionary<int, bool> _blinking;
        public static bool IsBlinking(this SpriteRenderer spriteRenderer)
        {
            _blinking ??= new Dictionary<int, bool>();
        
            int id = spriteRenderer.GetHashCode();
            if (!_blinking.ContainsKey(id)) _blinking.Add(id , false);
        
            return _blinking[id];
        }
        public static bool IsBlinking(this Image spriteRenderer)
        {
            _blinking ??= new Dictionary<int, bool>();
        
            int id = spriteRenderer.GetHashCode();
            if (!_blinking.ContainsKey(id)) _blinking.Add(id , false);
        
            return _blinking[id];
        } 
    
        /// <summary>
        /// <para>Makes a blinking effect to the object</para>
        /// <param name="duration">The duration in seconds of the blinking effect</param>
        /// <param name="ticks">The number of times you want the object to blink</param>
        /// <param name="targetColor">The color to change in every blink, normally transparent or white</param>
        /// </summary>
        public static void DoBlink(this SpriteRenderer spriteRenderer, float duration, int ticks, Color targetColor)
            => CoroutineController.Start(DoBlinkEnumerator(spriteRenderer, duration, ticks, targetColor));
        private static IEnumerator DoBlinkEnumerator(SpriteRenderer spriteRenderer, float duration, int ticks, Color targetColor)
        {
            if (ticks <= 0) yield break;
        
            _blinking ??= new Dictionary<int, bool>();
            int id = spriteRenderer.GetHashCode();
            if(!_blinking.ContainsKey(id)) _blinking.Add( id , true);
            else _blinking[id] = true;

            float timer = 0;
            Color initialColor = spriteRenderer.color;
            WaitForSeconds waitForSeconds = new WaitForSeconds(duration / ticks/2);
        
            while (timer<duration)
            {
                initialColor = spriteRenderer.color;
                spriteRenderer.color = targetColor;
                yield return waitForSeconds;
                spriteRenderer.color = initialColor;
                yield return waitForSeconds;
                timer += duration / ticks;
            }

            spriteRenderer.color = initialColor;
            _blinking[id] = false;
        }
        
        /// <summary>
        /// <para>Makes a blinking effect to the object</para>
        /// <param name="duration">The duration in seconds of the blinking effect</param>
        /// <param name="ticks">The number of times you want the object to blink</param>
        /// <param name="targetColor">The color to change in every blink, normally transparent or white</param>
        /// </summary>
        public static async Task DoBlinkAsync(this SpriteRenderer spriteRenderer, float duration, int ticks, Color targetColor)
        {
            if (ticks <= 0) return;

            float timer = 0;
            Color initialColor = spriteRenderer.color;
            int waitForSeconds = (int)(duration/ticks/2*1000);
        
            while (timer<duration)
            {
                initialColor = spriteRenderer.color;
                spriteRenderer.color = targetColor;
                await UnityDelay.Delay(waitForSeconds);
                spriteRenderer.color = initialColor;
                await UnityDelay.Delay(waitForSeconds);
                timer += duration / ticks;
            }

            spriteRenderer.color = initialColor;
        }
    
        /// <summary>
        /// <para>Makes a blinking effect to the object</para>
        /// <param name="duration">The duration in seconds of the blinking effect</param>
        /// <param name="ticks">The number of times you want the object to blink</param>
        /// <param name="targetColor">The color to change in every blink, normally transparent or white</param>
        /// </summary>
        public static void DoBlink(this Image image, float duration, int ticks, Color targetColor) => 
            CoroutineController.Start(DoBlinkEnumerator(image, duration, ticks, targetColor));
        private static IEnumerator DoBlinkEnumerator(Image image, float duration, int ticks, Color targetColor)
        {
            if (ticks <= 0) yield break;
        
            _blinking ??= new Dictionary<int, bool>();
            int id = image.GetHashCode();
            if(!_blinking.ContainsKey(id)) _blinking.Add( id , true);
            else _blinking[id] = true;

            float timer = 0;
            Color initialColor = image.color;
            WaitForSeconds waitForSeconds = new WaitForSeconds(duration / ticks/2);
        
            while (timer<duration)
            {
                initialColor = image.color;
                image.color = targetColor;
                yield return waitForSeconds;
                image.color = initialColor;
                yield return waitForSeconds;
                timer += duration / ticks;
            }

            image.color = initialColor;
            _blinking[id] = false;
        }
        
        /// <summary>
        /// <para>Makes a blinking effect to the object</para>
        /// <param name="duration">The duration in seconds of the blinking effect</param>
        /// <param name="ticks">The number of times you want the object to blink</param>
        /// <param name="targetColor">The color to change in every blink, normally transparent or white</param>
        /// </summary>
        public static async Task DoBlinkAsync(this Image image, float duration, int ticks, Color targetColor)
        {
            if (ticks <= 0) return;
            float timer = 0;
            Color initialColor = image.color;  
            int delay = (int)((duration/ticks/2)*1000);
        
            while (timer<duration)
            {
                initialColor = image.color;
                image.color = targetColor;
                await UnityDelay.Delay(delay);
                image.color = initialColor;
                await UnityDelay.Delay(delay);
                timer += duration / ticks;
            }

            image.color = initialColor;
        }

        
        /// <summary>
        /// <para>Makes the object transition from one color to another in a certain time</para>
        /// <param name="targetColor">The final color</param>
        /// <param name="duration">The duration in seconds of the transition</param>
        /// </summary>
        public static void DoChangeColor(this SpriteRenderer spriteRenderer, Color targetColor, float duration, TimeScales timeScale = TimeScales.Unscaled) 
            => CoroutineController.Start(DoChangeColorEnumerator(spriteRenderer, targetColor, duration, timeScale));
        private static IEnumerator DoChangeColorEnumerator(SpriteRenderer spriteRenderer, Color targetColor, float duration, TimeScales timeScale)
        {
            float timer = GetDeltaTime(timeScale);
            Color initialColor = spriteRenderer.color;
            Color colorDelta = targetColor - initialColor;
        
            while (timer<duration)
            {
                spriteRenderer.color = initialColor + colorDelta*(timer/duration);
                yield return null;
                timer += GetDeltaTime(timeScale);
            }

            spriteRenderer.color = targetColor;
        }
        
        /// <summary>
        /// <para>Makes the object transition from one color to another in a certain time</para>
        /// <param name="targetColor">The final color</param>
        /// <param name="duration">The duration in seconds of the transition</param>
        /// </summary>
        public static async Task DoChangeColorAsync(this SpriteRenderer spriteRenderer, Color targetColor, float duration, TimeScales timeScale = TimeScales.Unscaled)
        {
            float timer = GetDeltaTime(timeScale);
            Color initialColor = spriteRenderer.color;
            Color colorDelta = targetColor - initialColor;
        
            while (timer<duration)
            {
                spriteRenderer.color = initialColor + colorDelta*(timer/duration);
                await Task.Yield();
                timer += GetDeltaTime(timeScale);
            }

            spriteRenderer.color = targetColor;
        }
    
        /// <summary>
        /// <para>Makes the object transition from one color to another in a certain time</para>
        /// <param name="targetColor">The final color</param>
        /// <param name="duration">The duration in seconds of the transition</param>
        /// </summary>
        public static void DoChangeColor(this Image image, Color targetColor , float duration, TimeScales timeScale = TimeScales.Unscaled)
            => CoroutineController.Start(DoChangeColorEnumerator(image, targetColor, duration, timeScale));
        private static IEnumerator DoChangeColorEnumerator(Image image, Color targetColor, float duration, TimeScales timeScale)
        {
            float timer = GetDeltaTime(timeScale);
            Color initialColor = image.color;
            Color colorDelta = targetColor - initialColor;
        
            while (timer<duration)
            {
                image.color = initialColor + colorDelta*(timer/duration);
                yield return null;
                timer += GetDeltaTime(timeScale);
            }

            image.color = targetColor;
        }
        
        /// <summary>
        /// <para>Makes the object transition from one color to another in a certain time</para>
        /// <param name="targetColor">The final color</param>
        /// <param name="duration">The duration in seconds of the transition</param>
        /// </summary>
        public static async Task DoChangeColorAsync(this Image image, Color targetColor, float duration, TimeScales timeScale = TimeScales.Unscaled)
        {
            float timer = GetDeltaTime(timeScale);
            Color initialColor = image.color;
            Color colorDelta = targetColor - initialColor;
        
            while (timer<duration)
            {
                image.color = initialColor + colorDelta*(timer/duration);
                await Task.Yield();
                timer += GetDeltaTime(timeScale);
            }

            image.color = targetColor;
        }

    }
}
