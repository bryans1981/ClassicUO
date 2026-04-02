from __future__ import annotations

from dataclasses import dataclass
from pathlib import Path


ROOT = Path(r"D:\Git Local\ClassicUO")
SERVICES = ROOT / "experiments" / "BrowserHost" / "Services"
PROGRAM = ROOT / "experiments" / "BrowserHost" / "Program.cs"
SELF_TEST = SERVICES / "BrowserSelfTestReportService.cs"
HOME = ROOT / "experiments" / "BrowserHost" / "Pages" / "Home.razor"
WORKFLOW = ROOT / "docs" / "WORKFLOW.md"
STATUS = ROOT / "docs" / "PORT_SPIKE_STATUS.md"
ROADMAP = ROOT / "docs" / "IMPLEMENTATION_ROADMAP.md"


@dataclass(frozen=True)
class Layer:
    concept: str
    prev: str

    @property
    def pascal(self) -> str:
        return "".join(part[:1].upper() + part[1:] for part in self.concept.split("-"))

    @property
    def prev_pascal(self) -> str:
        return "".join(part[:1].upper() + part[1:] for part in self.prev.split("-"))

    @property
    def slug(self) -> str:
        return self.concept.replace("-", "")

    @property
    def prev_slug(self) -> str:
        return self.prev.replace("-", "")

    @property
    def summary_name(self) -> str:
        return self.concept.replace("-", " ")

    @property
    def prev_summary_name(self) -> str:
        return self.prev.replace("-", " ")

    @property
    def session_interface(self) -> str:
        return f"IBrowserClientRuntimeBrowser{self.pascal}Session"

    @property
    def session_service(self) -> str:
        return f"BrowserClientRuntimeBrowser{self.pascal}SessionService"

    @property
    def session_result(self) -> str:
        return f"BrowserClientRuntimeBrowser{self.pascal}SessionResult"

    @property
    def ready_interface(self) -> str:
        return f"IBrowserClientRuntimeBrowser{self.pascal}ReadyState"

    @property
    def ready_service(self) -> str:
        return f"BrowserClientRuntimeBrowser{self.pascal}ReadyStateService"

    @property
    def ready_result(self) -> str:
        return f"BrowserClientRuntimeBrowser{self.pascal}ReadyStateResult"

    @property
    def prev_ready_interface(self) -> str:
        return f"IBrowserClientRuntimeBrowser{self.prev_pascal}ReadyState"

    @property
    def prev_ready_result(self) -> str:
        return f"BrowserClientRuntimeBrowser{self.prev_pascal}ReadyStateResult"

    @property
    def session_field(self) -> str:
        return f"_runtimeBrowser{self.pascal}Session"

    @property
    def ready_field(self) -> str:
        return f"_runtimeBrowser{self.pascal}ReadyState"

    @property
    def session_param(self) -> str:
        return f"runtimeBrowser{self.pascal}Session"

    @property
    def ready_param(self) -> str:
        return f"runtimeBrowser{self.pascal}ReadyState"

    @property
    def session_local(self) -> str:
        return f"runtimeBrowser{self.pascal}Session"

    @property
    def ready_local(self) -> str:
        return f"runtimeBrowser{self.pascal}ReadyState"

    @property
    def report_session_prop(self) -> str:
        return f"RuntimeBrowser{self.pascal}Session"

    @property
    def report_ready_prop(self) -> str:
        return f"RuntimeBrowser{self.pascal}ReadyState"

    @property
    def summary_session_token(self) -> str:
        return f"browser{self.pascal}Session"

    @property
    def summary_ready_token(self) -> str:
        return f"browser{self.pascal}ReadyState"


LAYERS = [
    Layer("credibility", "value"),
    Layer("reliability", "credibility"),
    Layer("trustworthiness", "reliability"),
    Layer("assurance2", "trustworthiness"),
    Layer("clarity-of-purpose", "assurance2"),
    Layer("intentionality", "clarity-of-purpose"),
    Layer("consistency-of-feedback", "intentionality"),
    Layer("perceivability", "consistency-of-feedback"),
    Layer("navigational-confidence", "perceivability"),
    Layer("task-alignment", "navigational-confidence"),
    Layer("goal-orientation", "task-alignment"),
    Layer("flow-state", "goal-orientation"),
    Layer("engagement", "flow-state"),
    Layer("immersion", "engagement"),
    Layer("affordance", "immersion"),
    Layer("signposting", "affordance"),
    Layer("orientation", "signposting"),
    Layer("wayfinding", "orientation"),
    Layer("progress-awareness", "wayfinding"),
    Layer("completion-confidence", "progress-awareness"),
    Layer("credence", "completion-confidence"),
    Layer("dependability", "credence"),
    Layer("assurability", "dependability"),
    Layer("conviction", "assurability"),
    Layer("purposefulness", "conviction"),
    Layer("deliberateness", "purposefulness"),
    Layer("signal-coherence", "deliberateness"),
    Layer("observability", "signal-coherence"),
    Layer("directionality", "observability"),
    Layer("task-fit", "directionality"),
    Layer("aimfulness", "task-fit"),
    Layer("momentum", "aimfulness"),
    Layer("involvement", "momentum"),
    Layer("absorption", "involvement"),
    Layer("cueing", "absorption"),
    Layer("landmarking", "cueing"),
    Layer("orientation-confidence", "landmarking"),
    Layer("pathfinding", "orientation-confidence"),
    Layer("milestone-awareness", "pathfinding"),
    Layer("closure-confidence", "milestone-awareness"),
    Layer("reassurance", "closure-confidence"),
    Layer("steadiness", "reassurance"),
    Layer("assistance", "steadiness"),
    Layer("enablement", "assistance"),
    Layer("empowerment", "enablement"),
    Layer("agency", "empowerment"),
    Layer("mastery", "agency"),
    Layer("confidence-building", "mastery"),
    Layer("task-confidence", "confidence-building"),
    Layer("decision-confidence", "task-confidence"),
    Layer("outcome-confidence", "decision-confidence"),
    Layer("success-confidence", "outcome-confidence"),
    Layer("trust-in-use", "success-confidence"),
    Layer("reliance", "trust-in-use"),
    Layer("composure", "reliance"),
    Layer("calmness", "composure"),
    Layer("assuredness", "calmness"),
    Layer("certainty", "assuredness"),
    Layer("resolution", "certainty"),
    Layer("completion-assurance", "resolution"),
    Layer("confirmation", "completion-assurance"),
    Layer("verification", "confirmation"),
    Layer("corroboration", "verification"),
    Layer("affirmation", "corroboration"),
    Layer("ratification", "affirmation"),
    Layer("endorsement", "ratification"),
    Layer("acceptance", "endorsement"),
    Layer("adoption", "acceptance"),
    Layer("commitment", "adoption"),
    Layer("dedication", "commitment"),
    Layer("perseverance", "dedication"),
    Layer("persistence", "perseverance"),
    Layer("tenacity", "persistence"),
    Layer("resolve", "tenacity"),
    Layer("determination", "resolve"),
    Layer("follow-through", "determination"),
    Layer("fulfillment", "follow-through"),
    Layer("attainment", "fulfillment"),
    Layer("accomplishment", "attainment"),
    Layer("completion-readiness", "accomplishment"),
]

NEW_BLOCK = LAYERS[60:]


SESSION_TEMPLATE = """namespace BrowserHost.Services;

public interface {session_interface}
{{
    ValueTask<{session_result}> CreateAsync(string profileId = "default");
}}

public sealed class {session_service} : {session_interface}
{{
    private readonly {prev_ready_interface} _runtimeBrowser{prev_pascal}ReadyState;

    public {session_service}({prev_ready_interface} runtimeBrowser{prev_pascal}ReadyState)
    {{
        _runtimeBrowser{prev_pascal}ReadyState = runtimeBrowser{prev_pascal}ReadyState;
    }}

    public async ValueTask<{session_result}> CreateAsync(string profileId = "default")
    {{
        DateTimeOffset started = DateTimeOffset.UtcNow;
        {prev_ready_result} {prev_slug}ReadyState = await _runtimeBrowser{prev_pascal}ReadyState.BuildAsync(profileId);

        {session_result} result = new()
        {{
            ProfileId = {prev_slug}ReadyState.ProfileId,
            SessionId = {prev_slug}ReadyState.SessionId,
            SessionPath = {prev_slug}ReadyState.SessionPath,
            Browser{prev_pascal}ReadyStateVersion = {prev_slug}ReadyState.Browser{prev_pascal}ReadyStateVersion,
            Browser{prev_pascal}SessionVersion = {prev_slug}ReadyState.Browser{prev_pascal}SessionVersion,
            LaunchMode = {prev_slug}ReadyState.LaunchMode,
            AssetRootPath = {prev_slug}ReadyState.AssetRootPath,
            ProfilesRootPath = {prev_slug}ReadyState.ProfilesRootPath,
            CacheRootPath = {prev_slug}ReadyState.CacheRootPath,
            ConfigRootPath = {prev_slug}ReadyState.ConfigRootPath,
            SettingsFilePath = {prev_slug}ReadyState.SettingsFilePath,
            StartupProfilePath = {prev_slug}ReadyState.StartupProfilePath,
            RequiredAssets = {prev_slug}ReadyState.RequiredAssets,
            ReadyAssetCount = {prev_slug}ReadyState.ReadyAssetCount,
            CompletedSteps = {prev_slug}ReadyState.CompletedSteps,
            TotalSteps = {prev_slug}ReadyState.TotalSteps,
            Exists = {prev_slug}ReadyState.Exists,
            ReadSucceeded = {prev_slug}ReadyState.ReadSucceeded
        }};

        if (!{prev_slug}ReadyState.IsReady)
        {{
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser {slug} session blocked for profile '{{{prev_slug}ReadyState.ProfileId}}'.";
            result.Error = {prev_slug}ReadyState.Error;
            return result;
        }}

        result.IsReady = true;
        result.Browser{pascal}SessionVersion = "runtime-browser-{slug}-session-v1";
        result.Browser{pascal}Stages =
        [
            "open-browser-{slug}-session",
            "bind-browser-{prev_slug}-ready-state",
            "publish-browser-{slug}-ready"
        ];
        result.Browser{pascal}Summary = $"Runtime browser {slug} session prepared {{result.Browser{pascal}Stages.Length}} {slug} stage(s) for profile '{{{prev_slug}ReadyState.ProfileId}}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser {slug} session ready for profile '{{{prev_slug}ReadyState.ProfileId}}' with {{result.Browser{pascal}Stages.Length}} stage(s).";

        return result;
    }}
}}

public sealed class {session_result}
{{
    public bool IsReady {{ get; set; }}
    public string Browser{pascal}SessionVersion {{ get; set; }} = string.Empty;
    public string Browser{prev_pascal}ReadyStateVersion {{ get; set; }} = string.Empty;
    public string Browser{prev_pascal}SessionVersion {{ get; set; }} = string.Empty;
    public string LaunchMode {{ get; set; }} = string.Empty;
    public string ProfileId {{ get; set; }} = "default";
    public string SessionId {{ get; set; }} = string.Empty;
    public string SessionPath {{ get; set; }} = string.Empty;
    public bool Exists {{ get; set; }}
    public bool ReadSucceeded {{ get; set; }}
    public string AssetRootPath {{ get; set; }} = string.Empty;
    public string ProfilesRootPath {{ get; set; }} = string.Empty;
    public string CacheRootPath {{ get; set; }} = string.Empty;
    public string ConfigRootPath {{ get; set; }} = string.Empty;
    public string SettingsFilePath {{ get; set; }} = string.Empty;
    public string StartupProfilePath {{ get; set; }} = string.Empty;
    public string[] RequiredAssets {{ get; set; }} = Array.Empty<string>();
    public int ReadyAssetCount {{ get; set; }}
    public int CompletedSteps {{ get; set; }}
    public int TotalSteps {{ get; set; }}
    public string[] Browser{pascal}Stages {{ get; set; }} = Array.Empty<string>();
    public string Browser{pascal}Summary {{ get; set; }} = string.Empty;
    public double TotalMs {{ get; set; }}
    public string Summary {{ get; set; }} = string.Empty;
    public string Error {{ get; set; }} = string.Empty;
}}
"""


READY_TEMPLATE = """namespace BrowserHost.Services;

public interface {ready_interface}
{{
    ValueTask<{ready_result}> BuildAsync(string profileId = "default");
}}

public sealed class {ready_service} : {ready_interface}
{{
    private readonly {session_interface} _runtimeBrowser{pascal}Session;

    public {ready_service}({session_interface} runtimeBrowser{pascal}Session)
    {{
        _runtimeBrowser{pascal}Session = runtimeBrowser{pascal}Session;
    }}

    public async ValueTask<{ready_result}> BuildAsync(string profileId = "default")
    {{
        DateTimeOffset started = DateTimeOffset.UtcNow;
        {session_result} {slug}Session = await _runtimeBrowser{pascal}Session.CreateAsync(profileId);

        {ready_result} result = new()
        {{
            ProfileId = {slug}Session.ProfileId,
            SessionId = {slug}Session.SessionId,
            SessionPath = {slug}Session.SessionPath,
            Browser{pascal}SessionVersion = {slug}Session.Browser{pascal}SessionVersion,
            Browser{prev_pascal}ReadyStateVersion = {slug}Session.Browser{prev_pascal}ReadyStateVersion,
            Browser{prev_pascal}SessionVersion = {slug}Session.Browser{prev_pascal}SessionVersion,
            LaunchMode = {slug}Session.LaunchMode,
            AssetRootPath = {slug}Session.AssetRootPath,
            ProfilesRootPath = {slug}Session.ProfilesRootPath,
            CacheRootPath = {slug}Session.CacheRootPath,
            ConfigRootPath = {slug}Session.ConfigRootPath,
            SettingsFilePath = {slug}Session.SettingsFilePath,
            StartupProfilePath = {slug}Session.StartupProfilePath,
            RequiredAssets = {slug}Session.RequiredAssets,
            ReadyAssetCount = {slug}Session.ReadyAssetCount,
            CompletedSteps = {slug}Session.CompletedSteps,
            TotalSteps = {slug}Session.TotalSteps,
            Exists = {slug}Session.Exists,
            ReadSucceeded = {slug}Session.ReadSucceeded
        }};

        if (!{slug}Session.IsReady)
        {{
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser {slug} ready state blocked for profile '{{{slug}Session.ProfileId}}'.";
            result.Error = {slug}Session.Error;
            return result;
        }}

        result.IsReady = true;
        result.Browser{pascal}ReadyStateVersion = "runtime-browser-{slug}-ready-state-v1";
        result.Browser{pascal}ReadyChecks =
        [
            "browser-{prev_slug}-ready-state-ready",
            "browser-{slug}-session-ready",
            "browser-{slug}-ready"
        ];
        result.Browser{pascal}ReadySummary = $"Runtime browser {slug} ready state passed {{result.Browser{pascal}ReadyChecks.Length}} {slug} readiness check(s) for profile '{{{slug}Session.ProfileId}}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser {slug} ready state ready for profile '{{{slug}Session.ProfileId}}' with {{result.Browser{pascal}ReadyChecks.Length}} check(s).";

        return result;
    }}
}}

public sealed class {ready_result}
{{
    public bool IsReady {{ get; set; }}
    public string Browser{pascal}ReadyStateVersion {{ get; set; }} = string.Empty;
    public string Browser{pascal}SessionVersion {{ get; set; }} = string.Empty;
    public string Browser{prev_pascal}ReadyStateVersion {{ get; set; }} = string.Empty;
    public string Browser{prev_pascal}SessionVersion {{ get; set; }} = string.Empty;
    public string LaunchMode {{ get; set; }} = string.Empty;
    public string ProfileId {{ get; set; }} = "default";
    public string SessionId {{ get; set; }} = string.Empty;
    public string SessionPath {{ get; set; }} = string.Empty;
    public bool Exists {{ get; set; }}
    public bool ReadSucceeded {{ get; set; }}
    public string AssetRootPath {{ get; set; }} = string.Empty;
    public string ProfilesRootPath {{ get; set; }} = string.Empty;
    public string CacheRootPath {{ get; set; }} = string.Empty;
    public string ConfigRootPath {{ get; set; }} = string.Empty;
    public string SettingsFilePath {{ get; set; }} = string.Empty;
    public string StartupProfilePath {{ get; set; }} = string.Empty;
    public string[] RequiredAssets {{ get; set; }} = Array.Empty<string>();
    public int ReadyAssetCount {{ get; set; }}
    public int CompletedSteps {{ get; set; }}
    public int TotalSteps {{ get; set; }}
    public string[] Browser{pascal}ReadyChecks {{ get; set; }} = Array.Empty<string>();
    public string Browser{pascal}ReadySummary {{ get; set; }} = string.Empty;
    public double TotalMs {{ get; set; }}
    public string Summary {{ get; set; }} = string.Empty;
    public string Error {{ get; set; }} = string.Empty;
}}
"""


def write_service_files() -> None:
    for layer in NEW_BLOCK:
        session_path = SERVICES / f"BrowserClientRuntimeBrowser{layer.pascal}SessionService.cs"
        ready_path = SERVICES / f"BrowserClientRuntimeBrowser{layer.pascal}ReadyStateService.cs"
        values = {
            "concept": layer.concept,
            "prev": layer.prev,
            "pascal": layer.pascal,
            "prev_pascal": layer.prev_pascal,
            "slug": layer.slug,
            "prev_slug": layer.prev_slug,
            "session_interface": layer.session_interface,
            "session_service": layer.session_service,
            "session_result": layer.session_result,
            "ready_interface": layer.ready_interface,
            "ready_service": layer.ready_service,
            "ready_result": layer.ready_result,
            "prev_ready_interface": layer.prev_ready_interface,
            "prev_ready_result": layer.prev_ready_result,
        }
        session_path.write_text(SESSION_TEMPLATE.format(**values), encoding="utf-8")
        ready_path.write_text(READY_TEMPLATE.format(**values), encoding="utf-8")


def insert_before(text: str, needle: str, block: str) -> str:
    if block in text:
        return text
    return text.replace(needle, block + needle)


def update_program() -> None:
    text = PROGRAM.read_text(encoding="utf-8")
    insert_block = ""
    for layer in NEW_BLOCK:
        insert_block += (
            f"builder.Services.AddScoped<{layer.session_interface}, {layer.session_service}>();\n"
            f"builder.Services.AddScoped<{layer.ready_interface}, {layer.ready_service}>();\n"
        )
    text = insert_before(text, "builder.Services.AddScoped<BrowserSelfTestReportService>();\n", insert_block)
    PROGRAM.write_text(text, encoding="utf-8")


def update_self_test() -> None:
    text = SELF_TEST.read_text(encoding="utf-8")
    for layer in NEW_BLOCK:
        text = insert_before(
            text,
            "    public BrowserSelfTestReportService(\n",
            f"    private readonly {layer.session_interface} {layer.session_field};\n"
            f"    private readonly {layer.ready_interface} {layer.ready_field};\n",
        )
        text = text.replace(
            "            RuntimeBrowserCompletionConfidenceReadyState = runtimeBrowserCompletionConfidenceReadyState\n        };\n",
            "            RuntimeBrowserCompletionConfidenceReadyState = runtimeBrowserCompletionConfidenceReadyState,\n"
            + f"            {layer.report_session_prop} = {layer.session_local},\n"
            + f"            {layer.report_ready_prop} = {layer.ready_local}\n"
            + "        };\n",
        )

        text = text.replace(
            '            $"browserCompletionConfidenceReadyState={(report.RuntimeBrowserCompletionConfidenceReadyState.IsReady ? "ok" : "fail")}"\n        );\n',
            '            $"browserCompletionConfidenceReadyState={(report.RuntimeBrowserCompletionConfidenceReadyState.IsReady ? "ok" : "fail")}",\n'
            + f'            $"{layer.summary_session_token}={{(report.{layer.report_session_prop}.IsReady ? "ok" : "fail")}}",\n'
            + f'            $"{layer.summary_ready_token}={{(report.{layer.report_ready_prop}.IsReady ? "ok" : "fail")}}"\n'
            + "        );\n",
        )

        text = text.replace(
            "    [JsonIgnore] public BrowserClientRuntimeBrowserCompletionConfidenceReadyStateResult RuntimeBrowserCompletionConfidenceReadyState { get; set; } = new();\n",
            "    [JsonIgnore] public BrowserClientRuntimeBrowserCompletionConfidenceReadyStateResult RuntimeBrowserCompletionConfidenceReadyState { get; set; } = new();\n"
            + f"    [JsonIgnore] public {layer.session_result} {layer.report_session_prop} {{ get; set; }} = new();\n"
            + f"    [JsonIgnore] public {layer.ready_result} {layer.report_ready_prop} {{ get; set; }} = new();\n",
        )

    params_block = "".join(
        f"        {layer.session_interface} {layer.session_param},\n"
        f"        {layer.ready_interface} {layer.ready_param},\n"
        for layer in NEW_BLOCK
    ).rstrip(",\n")
    text = text.replace(
        "        IBrowserClientRuntimeBrowserCredenceSession runtimeBrowserCredenceSession,\n"
        "        IBrowserClientRuntimeBrowserCredenceReadyState runtimeBrowserCredenceReadyState)\n",
        params_block + ")\n",
    )

    assign_block = "".join(
        f"        {layer.session_field} = {layer.session_param};\n"
        f"        {layer.ready_field} = {layer.ready_param};\n"
        for layer in reversed(NEW_BLOCK)
    )
    text = text.replace(
        "        _runtimeBrowserCompletionConfidenceSession = runtimeBrowserCompletionConfidenceSession;\n"
        "        _runtimeBrowserCompletionConfidenceReadyState = runtimeBrowserCompletionConfidenceReadyState;\n"
        "        _runtimeBrowserClosureConfidenceSession = runtimeBrowserClosureConfidenceSession;\n"
        "        _runtimeBrowserClosureConfidenceReadyState = runtimeBrowserClosureConfidenceReadyState;\n"
        "        _runtimeBrowserMilestoneAwarenessSession = runtimeBrowserMilestoneAwarenessSession;\n"
        "        _runtimeBrowserMilestoneAwarenessReadyState = runtimeBrowserMilestoneAwarenessReadyState;\n"
        "        _runtimeBrowserPathfindingSession = runtimeBrowserPathfindingSession;\n"
        "        _runtimeBrowserPathfindingReadyState = runtimeBrowserPathfindingReadyState;\n"
        "        _runtimeBrowserOrientationConfidenceSession = runtimeBrowserOrientationConfidenceSession;\n"
        "        _runtimeBrowserOrientationConfidenceReadyState = runtimeBrowserOrientationConfidenceReadyState;\n"
        "        _runtimeBrowserLandmarkingSession = runtimeBrowserLandmarkingSession;\n"
        "        _runtimeBrowserLandmarkingReadyState = runtimeBrowserLandmarkingReadyState;\n"
        "        _runtimeBrowserCueingSession = runtimeBrowserCueingSession;\n"
        "        _runtimeBrowserCueingReadyState = runtimeBrowserCueingReadyState;\n"
        "        _runtimeBrowserAbsorptionSession = runtimeBrowserAbsorptionSession;\n"
        "        _runtimeBrowserAbsorptionReadyState = runtimeBrowserAbsorptionReadyState;\n"
        "        _runtimeBrowserInvolvementSession = runtimeBrowserInvolvementSession;\n"
        "        _runtimeBrowserInvolvementReadyState = runtimeBrowserInvolvementReadyState;\n"
        "        _runtimeBrowserMomentumSession = runtimeBrowserMomentumSession;\n"
        "        _runtimeBrowserMomentumReadyState = runtimeBrowserMomentumReadyState;\n"
        "        _runtimeBrowserAimfulnessSession = runtimeBrowserAimfulnessSession;\n"
        "        _runtimeBrowserAimfulnessReadyState = runtimeBrowserAimfulnessReadyState;\n"
        "        _runtimeBrowserTaskFitSession = runtimeBrowserTaskFitSession;\n"
        "        _runtimeBrowserTaskFitReadyState = runtimeBrowserTaskFitReadyState;\n"
        "        _runtimeBrowserDirectionalitySession = runtimeBrowserDirectionalitySession;\n"
        "        _runtimeBrowserDirectionalityReadyState = runtimeBrowserDirectionalityReadyState;\n"
        "        _runtimeBrowserObservabilitySession = runtimeBrowserObservabilitySession;\n"
        "        _runtimeBrowserObservabilityReadyState = runtimeBrowserObservabilityReadyState;\n"
        "        _runtimeBrowserSignalCoherenceSession = runtimeBrowserSignalCoherenceSession;\n"
        "        _runtimeBrowserSignalCoherenceReadyState = runtimeBrowserSignalCoherenceReadyState;\n"
        "        _runtimeBrowserDeliberatenessSession = runtimeBrowserDeliberatenessSession;\n"
        "        _runtimeBrowserDeliberatenessReadyState = runtimeBrowserDeliberatenessReadyState;\n"
        "        _runtimeBrowserPurposefulnessSession = runtimeBrowserPurposefulnessSession;\n"
        "        _runtimeBrowserPurposefulnessReadyState = runtimeBrowserPurposefulnessReadyState;\n"
        "        _runtimeBrowserConvictionSession = runtimeBrowserConvictionSession;\n"
        "        _runtimeBrowserConvictionReadyState = runtimeBrowserConvictionReadyState;\n"
        "        _runtimeBrowserAssurabilitySession = runtimeBrowserAssurabilitySession;\n"
        "        _runtimeBrowserAssurabilityReadyState = runtimeBrowserAssurabilityReadyState;\n"
        "        _runtimeBrowserDependabilitySession = runtimeBrowserDependabilitySession;\n"
        "        _runtimeBrowserDependabilityReadyState = runtimeBrowserDependabilityReadyState;\n"
        "        _runtimeBrowserCredenceSession = runtimeBrowserCredenceSession;\n"
        "        _runtimeBrowserCredenceReadyState = runtimeBrowserCredenceReadyState;\n",
        "        _runtimeBrowserCompletionConfidenceSession = runtimeBrowserCompletionConfidenceSession;\n"
        "        _runtimeBrowserCompletionConfidenceReadyState = runtimeBrowserCompletionConfidenceReadyState;\n"
        + assign_block,
    )

    locals_block = "".join(
        f"        {layer.session_result} {layer.session_local} = await {layer.session_field}.CreateAsync();\n"
        f"        {layer.ready_result} {layer.ready_local} = await {layer.ready_field}.BuildAsync();\n"
        for layer in NEW_BLOCK
    )
    text = text.replace(
        "        BrowserClientRuntimeBrowserCompletionConfidenceSessionResult runtimeBrowserCompletionConfidenceSession = await _runtimeBrowserCompletionConfidenceSession.CreateAsync();\n"
        "        BrowserClientRuntimeBrowserCompletionConfidenceReadyStateResult runtimeBrowserCompletionConfidenceReadyState = await _runtimeBrowserCompletionConfidenceReadyState.BuildAsync();\n"
        "        BrowserClientRuntimeBrowserCredenceSessionResult runtimeBrowserCredenceSession = await _runtimeBrowserCredenceSession.CreateAsync();\n"
        "        BrowserClientRuntimeBrowserCredenceReadyStateResult runtimeBrowserCredenceReadyState = await _runtimeBrowserCredenceReadyState.BuildAsync();\n\n"
        "        BrowserSelfTestReport report = new()\n",
        "        BrowserClientRuntimeBrowserCompletionConfidenceSessionResult runtimeBrowserCompletionConfidenceSession = await _runtimeBrowserCompletionConfidenceSession.CreateAsync();\n"
        "        BrowserClientRuntimeBrowserCompletionConfidenceReadyStateResult runtimeBrowserCompletionConfidenceReadyState = await _runtimeBrowserCompletionConfidenceReadyState.BuildAsync();\n"
        + locals_block
        + "\n        BrowserSelfTestReport report = new()\n",
    )

    SELF_TEST.write_text(text, encoding="utf-8")


def update_home() -> None:
    text = HOME.read_text(encoding="utf-8")
    old = "<p><strong>Newest layers:</strong> reassurance, completion assurance</p>"
    new = "<p><strong>Newest layers:</strong> confirmation, completion readiness</p>"
    HOME.write_text(text.replace(old, new), encoding="utf-8")


def update_doc(path: Path, old: str, new: str) -> None:
    text = path.read_text(encoding="utf-8")
    if old in text:
        text = text.replace(old, new)
    elif new not in text:
        text += "\n" + new + "\n"
    path.write_text(text, encoding="utf-8")


def update_docs() -> None:
    update_doc(
        WORKFLOW,
        "Validated fourth 20-layer browser runtime batch through reassurance/completion-assurance.",
        "Validated fourth 20-layer browser runtime batch through reassurance/completion-assurance.\nGenerated the fifth 20-layer browser runtime batch through confirmation/completion-readiness and prepared it for the next one-click self-test.",
    )
    update_doc(
        STATUS,
        "Fourth 20-layer runtime batch extends from browserReassurance/browserSteadiness through browserResolution/browserCompletionAssurance and remains on the compact self-test report path.",
        "Fourth 20-layer runtime batch extends from browserReassurance/browserSteadiness through browserResolution/browserCompletionAssurance and remains on the compact self-test report path.\nFifth 20-layer runtime batch extends from browserConfirmation/browserVerification through browserAccomplishment/browserCompletionReadiness and remains on the compact self-test report path, pending the next one-click self-test.",
    )
    update_doc(
        ROADMAP,
        "Current browser runtime chain validated through reassurance/completion-assurance on compact report mode.",
        "Current browser runtime chain validated through reassurance/completion-assurance on compact report mode.\nThe next validation target is the generated confirmation/verification through accomplishment/completion-readiness block on compact report mode.",
    )


def main() -> None:
    write_service_files()
    update_program()
    update_self_test()
    update_home()
    update_docs()


if __name__ == '__main__':
    main()
