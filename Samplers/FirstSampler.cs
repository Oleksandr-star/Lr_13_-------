using OpenTelemetry.Trace;

namespace Lb13.Samplers
{
    public class FirstSampler : Sampler
    {
        private readonly double _sampleRate;

        public FirstSampler(double sampleRate)
        {
            if (sampleRate < 0.0 || sampleRate > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(sampleRate), "Sample rate must be between 0.0 and 1.0");
            }

            _sampleRate = sampleRate;
        }

        public override SamplingResult ShouldSample(in SamplingParameters samplingParameters)
        {
            var random = new Random().NextDouble();

            if (random < _sampleRate)
            {
                return new SamplingResult(SamplingDecision.RecordAndSample);
            }
            else
            {
                return new SamplingResult(SamplingDecision.Drop);
            }
        }
    }
}
